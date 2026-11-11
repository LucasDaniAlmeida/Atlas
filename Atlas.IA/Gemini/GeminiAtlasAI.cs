using Atlas.Core.Models;
using Atlas.IA.Capabilities;
using Atlas.IA.Conversation;
using Atlas.IA.Interfaces;
using Atlas.IA.Models;
using Google.GenAI;
using Google.GenAI.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Atlas.IA.Gemini;

public class GeminiAtlasAI : IAtlasAI
{
    private readonly Client _client;
    private readonly IConversationHistory _history;



    public GeminiAtlasAI(IConversationHistory history)
    {
        _client = new Client();
        _history = history;



    }

    private async Task<AtlasResponse> GenerateAsync(
    List<Content> contents)
    {


        var config = new GenerateContentConfig
        {
            SystemInstruction = new Content
            {
                Parts =
                [
                    new Part
                {
                    Text = $"""
                    {AtlasSystemInstruction.Text}

                    {AtlasCapabilities.Text}
                    """
                }
                ]
            },

            ResponseMimeType = "application/json"
        };

        var response =
    await _client.Models.GenerateContentAsync(
        model: "gemini-3.5-flash-lite",
        contents: contents,
        config: config
    );

        string? json =
            response.Candidates?[0]
                .Content?
                .Parts?[0]
                .Text;

        if (string.IsNullOrWhiteSpace(json))
            return new AtlasResponse();

        return JsonSerializer.Deserialize<AtlasResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        ) ?? new AtlasResponse();
    }

    public async Task<AtlasResponse> ProcessAsync(
        string input,
        ConversationSession session)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);
        ArgumentNullException.ThrowIfNull(session);

        var config = new GenerateContentConfig
        {
            SystemInstruction = new Content
            {
                Parts =
        [
            new Part
            {
                Text = $"""
                        {AtlasSystemInstruction.Text}

                        {AtlasCapabilities.Text}
                        """
            }
        ]
            },

            ResponseMimeType = "application/json"
        };

        var contents = new List<Content>();

        foreach (var conversationMessage in session.Messages)
        {
            contents.Add(new Content
            {
                Role = conversationMessage.Role == "assistant"
                    ? "model"
                    : "user",

                Parts =
                [
                    new Part
                    {
                        Text = conversationMessage.Content
                    }
                ]
            });
        }

        contents.Add(new Content
        {
            Role = "user",
            Parts =
            [
                new Part
                {
                    Text = input
                }
            ]
        });

        var atlasResponse = await GenerateAsync(contents);

        if (atlasResponse is null)
        {
            return new AtlasResponse();
        }

        var userMessage = new ConversationMessage
        {
            Role = "user",
            Content = input
        };

        session.Messages.Add(userMessage);
        await _history.AddAsync(userMessage);

        if (!string.IsNullOrWhiteSpace(atlasResponse.Message))
        {
            var assistantMessage = new ConversationMessage
            {
                Role = "assistant",
                Content = atlasResponse.Message
            };

            session.Messages.Add(assistantMessage);
            await _history.AddAsync(assistantMessage);
        }

        return atlasResponse;



    }

    public async Task<AtlasResponse> ProcessExecutionResultAsync(
     PlanResult result,
     ConversationSession session)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(session);

        var executionSummary = string.Join(
            "\n",
            result.Results.Select((r, index) =>
                $"Etapa {index + 1}: " +
                $"Success={r.Success}; " +
                $"Message={r.Message}"
            )
        );

        var contents = new List<Content>
    {
        new Content
        {
            Role = "user",
            Parts =
            [
                new Part
                {
                    Text = $"""
                    Este é um resultado interno de execução do Atlas.

                    Sucesso geral: {result.Success}

                    Resultados:
                    {executionSummary}

                    Não gere novas ações.

                    Se não houver nada útil para comunicar ao usuário,
                    retorne message = null e commands vazio.

                    Se houver falha ou algo relevante,
                    gere apenas uma mensagem curta e natural.
                    """
                }
            ]
        }
    };

        return await GenerateAsync(contents);
    }

}

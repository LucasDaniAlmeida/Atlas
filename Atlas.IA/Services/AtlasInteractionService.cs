using System;
using System.Collections.Generic;
using System.Text;
using Atlas.Core.Executor;
using Atlas.IA.Conversation;
using Atlas.IA.Interfaces;
using Atlas.IA.Models;

namespace Atlas.IA.Services;

public class AtlasInteractionService
{
    private readonly IAtlasAI _ai;
    private readonly PlanBuilder _planBuilder;
    private readonly IPlanExecutor _planExecutor;
    public AtlasResponse? Feedback { get; set; }

    public AtlasInteractionService(
        IAtlasAI ai,
        PlanBuilder planBuilder,
        IPlanExecutor planExecutor)
    {
        _ai = ai;
        _planBuilder = planBuilder;
        _planExecutor = planExecutor;
    }

    public async Task<AtlasInteractionResult> ProcessAsync(
        string input,
        ConversationSession session)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);
        ArgumentNullException.ThrowIfNull(session);

        var response =
            await _ai.ProcessAsync(input, session);

        if (response.Commands.Count == 0)
        {
            return new AtlasInteractionResult
            {
                Response = response
            };
        }

        var plan =
            _planBuilder.Build(response.Commands);

        var executionResult =
        await _planExecutor.ExecuteAsync(plan);

            var feedback =
                await _ai.ProcessExecutionResultAsync(
                    executionResult,
                    session
                );

            return new AtlasInteractionResult
            {
                Response = response,
                ExecutionResult = executionResult,
                Feedback = feedback
            };
    }
}
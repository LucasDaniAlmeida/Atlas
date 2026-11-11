using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Atlas.IA.Interfaces;
using Atlas.IA.Models;

namespace Atlas.IA.Conversation;

public class JsonConversationHistory : IConversationHistory
{
    private readonly string _filePath;

    private readonly SemaphoreSlim _lock = new(1, 1);

    public JsonConversationHistory(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        _filePath = filePath;
    }

    public async Task AddAsync(ConversationMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        await _lock.WaitAsync();

        try
        {
            var messages = await LoadAsync();

            messages.Add(message);

            await SaveAsync(messages);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IReadOnlyList<ConversationMessage>> GetLastAsync(int count)
    {
        if (count <= 0)
            return [];

        var messages = await LoadAsync();

        return messages
            .TakeLast(count)
            .ToList();
    }

    private async Task<List<ConversationMessage>> LoadAsync()
    {
        if (!File.Exists(_filePath))
            return [];

        var json = await File.ReadAllTextAsync(_filePath);

        if (string.IsNullOrWhiteSpace(json))
            return [];

        return JsonSerializer.Deserialize<List<ConversationMessage>>(json)
            ?? [];
    }

    private async Task SaveAsync(List<ConversationMessage> messages)
    {
        var directory = Path.GetDirectoryName(_filePath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(messages, options);

        await File.WriteAllTextAsync(_filePath, json);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Atlas.IA.Interfaces;
using Atlas.IA.Models;

namespace Atlas.IA.Conversation;

public class InMemoryConversationHistory : IConversationHistory
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly List<ConversationMessage> _messages = [];

    public Task AddAsync(ConversationMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        _messages.Add(message);

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<ConversationMessage>> GetLastAsync(int count)
    {
        if (count <= 0)
        {
            return Task.FromResult<IReadOnlyList<ConversationMessage>>(
                []
            );
        }

        var messages = _messages
            .TakeLast(count)
            .ToList();

        return Task.FromResult<IReadOnlyList<ConversationMessage>>(
            messages
        );
    }
}

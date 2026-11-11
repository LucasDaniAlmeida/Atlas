using System;
using System.Collections.Generic;
using System.Text;
using Atlas.IA.Models;

namespace Atlas.IA.Interfaces;

public interface IConversationHistory
{
    Task AddAsync(ConversationMessage message);

    Task<IReadOnlyList<ConversationMessage>> GetLastAsync(int count);
}

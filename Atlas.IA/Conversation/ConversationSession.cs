using System;
using System.Collections.Generic;
using System.Text;
using Atlas.IA.Models;

namespace Atlas.IA.Conversation;

public class ConversationSession
{
    public List<ConversationMessage> Messages { get; set; } = [];

    public void AddUserMessage(string content)
    {
        Messages.Add(new ConversationMessage
        {
            Role = "user",
            Content = content
        });
    }

    public void AddAssistantMessage(string content)
    {
        Messages.Add(new ConversationMessage
        {
            Role = "assistant",
            Content = content
        });
    }
}
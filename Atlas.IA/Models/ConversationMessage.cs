using System;
using System.Collections.Generic;
using System.Text;


namespace Atlas.IA.Models;

public class ConversationMessage
{
    public string Role { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.Now;
}

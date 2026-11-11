using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Core.Models
{
    public class CommandRequest
    {
        public string commandName { get; set; } = string.Empty;
        public CommandParameters Parameters { get; set; } = new();
        public CommandContext Context { get; set; } = new();
    }
}

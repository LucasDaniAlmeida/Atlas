using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Core.Models
{
    public class CommandResult
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
        public Exception? Exception { get; init; }

    }
}

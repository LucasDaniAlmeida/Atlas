using System;
using System.Collections.Generic;
using System.Text;




namespace Atlas.Core.Models;

public class PlanResult
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public List<CommandResult> Results { get; set; } = [];
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Core.Models;

public class ExecutionPlan
{
    public List<CommandRequest> Commands { get; set; } = [];
}

using System;
using System.Collections.Generic;
using System.Text;
using Atlas.Core.Models;

namespace Atlas.Core.Executor;

public interface IPlanExecutor
{
    Task<PlanResult> ExecuteAsync(ExecutionPlan plan);
}
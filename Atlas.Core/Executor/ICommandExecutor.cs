using System;
using System.Collections.Generic;
using System.Text;
using Atlas.Core.Models;

namespace Atlas.Core.Executor;

public interface ICommandExecutor
{
    Task<CommandResult> ExecuteAsync(CommandRequest request);
}
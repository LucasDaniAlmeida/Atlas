using Atlas.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Core.Commands.Interfaces;

public interface ICommand
{
    string CommandName { get; }

    Task<CommandResult> ExecuteAsync(CommandRequest request);
}

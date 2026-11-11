using System;
using System.Collections.Generic;
using System.Text;
using Atlas.Core.Commands.Interfaces;
using Atlas.Core.Models;
using Atlas.Core.Registry;

namespace Atlas.Core.Executor;

public class CommandExecutor : ICommandExecutor
{
    private readonly ICommandRegistry _registry;

    public CommandExecutor(ICommandRegistry registry)
    {
        _registry = registry;
    }

    public async Task<CommandResult> ExecuteAsync(CommandRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.commandName))
        {
            return new CommandResult
            {
                Success = false,
                Message = "O nome do comando não foi informado."
            };
        }

        if (!_registry.TryGet(request.commandName, out ICommand? command))
        {
            return new CommandResult
            {
                Success = false,
                Message = $"O comando '{request.commandName}' não foi encontrado."
            };
        }

        try
        {
            return await command.ExecuteAsync(request);
        }
        catch (Exception ex)
        {
            return new CommandResult
            {
                Success = false,
                Message = $"Erro ao executar o comando '{request.commandName}'.",
                Exception = ex
            };
        }
    }
}
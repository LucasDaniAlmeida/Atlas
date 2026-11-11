using Atlas.Core.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Core.Registry;

public class CommandRegistry : ICommandRegistry
{
    private readonly Dictionary<string, ICommand> _commands =
        new(StringComparer.OrdinalIgnoreCase);

    public void Register(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (string.IsNullOrWhiteSpace(command.CommandName))
            throw new ArgumentException(
                "O nome do comando não pode ser vazio.",
                nameof(command));

        if (_commands.ContainsKey(command.CommandName))
            throw new InvalidOperationException(
                $"O comando '{command.CommandName}' já está registrado.");

        _commands.Add(command.CommandName, command);
    }

    public bool TryGet(string commandName, out ICommand? command)
    {
        return _commands.TryGetValue(commandName, out command);
    }
}

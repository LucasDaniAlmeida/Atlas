using System;
using System.Collections.Generic;
using System.Text;
using Atlas.Core.Commands.Interfaces;
using Atlas.Core.Models;

namespace Atlas.Core.Commands.System;

public class Wait : ICommand
{
    public string CommandName => "wait";

    public async Task<CommandResult> ExecuteAsync(CommandRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        int? milliseconds = request.Parameters.Get<int>("milliseconds");

        if (milliseconds is null)
        {
            return new CommandResult
            {
                Success = false,
                Message = "O parâmetro 'milliseconds' não foi informado."
            };
        }

        if (milliseconds < 0)
        {
            return new CommandResult
            {
                Success = false,
                Message = "O parâmetro 'milliseconds' não pode ser negativo."
            };
        }

        try
        {
            await Task.Delay(milliseconds.Value);

            return new CommandResult
            {
                Success = true,
                Message = $"Aguardados {milliseconds.Value} ms."
            };
        }
        catch (Exception ex)
        {
            return new CommandResult
            {
                Success = false,
                Message = "Erro durante a espera.",
                Exception = ex
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Atlas.Core.Commands.Interfaces;
using Atlas.Core.Models;

namespace Atlas.Core.Commands.System;

public class OpenApplication : ICommand
{
    public string CommandName => "open_application";

    public Task<CommandResult> ExecuteAsync(CommandRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        string? application = request.Parameters.Get<string>("application");

        if (string.IsNullOrWhiteSpace(application))
        {
            return Task.FromResult(new CommandResult
            {
                Success = false,
                Message = "O parâmetro 'application' não foi informado."
            });
        }

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = application,
                UseShellExecute = true
            });

            return Task.FromResult(new CommandResult
            {
                Success = true,
                Message = $"Aplicação '{application}' iniciada."
            });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new CommandResult
            {
                Success = false,
                Message = $"Não foi possível iniciar a aplicação '{application}'.",
                Exception = ex
            });
        }
    }
}
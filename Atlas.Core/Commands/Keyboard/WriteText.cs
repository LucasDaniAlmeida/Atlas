using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Atlas.Core.Commands.Interfaces;
using Atlas.Core.Models;

namespace Atlas.Core.Commands.Keyboard;

public class WriteText : ICommand
{
    public string CommandName => "write_text";

    public Task<CommandResult> ExecuteAsync(CommandRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        string? text = request.Parameters.Get<string>("text");

        if (string.IsNullOrEmpty(text))
        {
            return Task.FromResult(new CommandResult
            {
                Success = false,
                Message = "O parâmetro 'text' não foi informado."
            });
        }

        try
        {
            SendKeys.SendWait(text);

            return Task.FromResult(new CommandResult
            {
                Success = true,
                Message = "Texto escrito com sucesso."
            });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new CommandResult
            {
                Success = false,
                Message = "Não foi possível escrever o texto.",
                Exception = ex
            });
        }
    }
}
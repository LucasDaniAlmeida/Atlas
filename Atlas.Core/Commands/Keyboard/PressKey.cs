using Atlas.Core.Commands.Interfaces;
using Atlas.Core.Models;
using System.Windows.Forms;

namespace Atlas.Core.Commands.Keyboard;

public class PressKey : ICommand
{
    public string CommandName => "press_key";

    public Task<CommandResult> ExecuteAsync(CommandRequest request)
    {
        if (!request.Parameters.Contains("key"))
        {
            return Task.FromResult(new CommandResult
            {
                Success = false,
                Message = "O parâmetro key é obrigatório."
            });
        }

        var key = request.Parameters.Get<string>("key");

        if (string.IsNullOrWhiteSpace(key))
        {
            return Task.FromResult(new CommandResult
            {
                Success = false,
                Message = "A tecla informada é inválida."
            });
        }

        try
        {
            SendKeys.SendWait(NormalizeKey(key));

            return Task.FromResult(new CommandResult
            {
                Success = true,
                Message = $"Tecla {key} pressionada."
            });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new CommandResult
            {
                Success = false,
                Message = $"Não foi possível pressionar a tecla {key}.",
                Exception = ex
            });
        }
    }

    private static string NormalizeKey(string key)
    {
        return key.Trim().ToUpperInvariant() switch
        {
            "ENTER" => "{ENTER}",
            "TAB" => "{TAB}",
            "ESC" => "{ESC}",
            "ESCAPE" => "{ESC}",
            "BACKSPACE" => "{BACKSPACE}",
            "DELETE" => "{DELETE}",
            "DEL" => "{DELETE}",
            "HOME" => "{HOME}",
            "END" => "{END}",
            "PAGEUP" => "{PGUP}",
            "PAGEDOWN" => "{PGDN}",
            "UP" => "{UP}",
            "DOWN" => "{DOWN}",
            "LEFT" => "{LEFT}",
            "RIGHT" => "{RIGHT}",
            "SPACE" => " ",
            _ => key
        };
    }
}
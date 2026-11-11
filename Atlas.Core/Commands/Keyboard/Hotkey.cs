using Atlas.Core.Commands.Interfaces;
using Atlas.Core.Models;
using System.Windows.Forms;

namespace Atlas.Core.Commands.Keyboard;

public class Hotkey : ICommand
{
    public string CommandName => "hotkey";

    public Task<CommandResult> ExecuteAsync(CommandRequest request)
    {
        if (!request.Parameters.Contains("keys"))
        {
            return Task.FromResult(new CommandResult
            {
                Success = false,
                Message = "O parâmetro keys é obrigatório."
            });
        }

        var keys = request.Parameters.Get<string>("keys");

        if (string.IsNullOrWhiteSpace(keys))
        {
            return Task.FromResult(new CommandResult
            {
                Success = false,
                Message = "A combinação de teclas informada é inválida."
            });
        }

        try
        {
            var sendKeysValue = ConvertHotkey(keys);

            SendKeys.SendWait(sendKeysValue);

            return Task.FromResult(new CommandResult
            {
                Success = true,
                Message = $"Atalho {keys} executado."
            });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new CommandResult
            {
                Success = false,
                Message = $"Não foi possível executar o atalho {keys}.",
                Exception = ex
            });
        }
    }

    private static string ConvertHotkey(string keys)
    {
        var parts = keys
            .Split('+', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.ToUpperInvariant())
            .ToList();

        var modifiers = string.Empty;
        var normalKeys = new List<string>();

        foreach (var part in parts)
        {
            switch (part)
            {
                case "CTRL":
                case "CONTROL":
                    modifiers += "^";
                    break;

                case "ALT":
                    modifiers += "%";
                    break;

                case "SHIFT":
                    modifiers += "+";
                    break;

                default:
                    normalKeys.Add(NormalizeKey(part));
                    break;
            }
        }

        if (normalKeys.Count == 0)
            throw new ArgumentException("Nenhuma tecla principal foi informada.");

        return modifiers + string.Concat(normalKeys);
    }

    private static string NormalizeKey(string key)
    {
        return key switch
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
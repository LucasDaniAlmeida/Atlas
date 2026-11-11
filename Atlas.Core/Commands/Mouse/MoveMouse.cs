using Atlas.Core.Commands.Interfaces;
using Atlas.Core.Models;
using System.Runtime.InteropServices;

namespace Atlas.Core.Commands.Mouse;

public class MoveMouse : ICommand
{
    public string CommandName => "move_mouse";

    public Task<CommandResult> ExecuteAsync(CommandRequest request)
    {
        if (!request.Parameters.Contains("x") ||
            !request.Parameters.Contains("y"))
        {
            return Task.FromResult(new CommandResult
            {
                Success = false,
                Message = "Os parâmetros x e y são obrigatórios."
            });
        }

        var x = request.Parameters.Get<int>("x");
        var y = request.Parameters.Get<int>("y");

        var moved = SetCursorPos(x, y);

        if (!moved)
        {
            return Task.FromResult(new CommandResult
            {
                Success = false,
                Message = $"Não foi possível mover o mouse para X={x}, Y={y}."
            });
        }

        return Task.FromResult(new CommandResult
        {
            Success = true,
            Message = $"Mouse movido para X={x}, Y={y}."
        });
    }

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int x, int y);
}
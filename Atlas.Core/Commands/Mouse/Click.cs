using Atlas.Core.Commands.Interfaces;
using Atlas.Core.Models;
using System.Runtime.InteropServices;

namespace Atlas.Core.Commands.Mouse;

public class Click : ICommand
{
    public string CommandName => "click";

    public async Task<CommandResult> ExecuteAsync(CommandRequest request)
    {
        var x = request.Parameters.Get<int>("x");
        var y = request.Parameters.Get<int>("y");

        if (!request.Parameters.Contains("x") ||
            !request.Parameters.Contains("y"))
        {
            return new CommandResult
            {
                Success = false,
                Message = "Os parâmetros x e y são obrigatórios."
            };
        }

        SetCursorPos(x, y);

        mouse_event(
            MOUSEEVENTF_LEFTDOWN,
            0,
            0,
            0,
            UIntPtr.Zero
        );

        mouse_event(
            MOUSEEVENTF_LEFTUP,
            0,
            0,
            0,
            UIntPtr.Zero
        );

        await Task.CompletedTask;

        return new CommandResult
        {
            Success = true,
            Message = $"Clique realizado em X={x}, Y={y}."
        };
    }

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    private static extern void mouse_event(
        uint dwFlags,
        uint dx,
        uint dy,
        uint dwData,
        UIntPtr dwExtraInfo
    );

    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;
}
namespace Atlas.Core.Authorization.Interfaces;

public interface ICommandConfirmationProvider
{
    Task<bool> ConfirmAsync(
        string title,
        string details,
        CancellationToken cancellationToken = default
    );
}
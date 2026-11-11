using Atlas.Core.Models;

namespace Atlas.Core.Authorization.Interfaces;

public interface ICommandAuthorizationService
{
    Task<CommandAuthorizationResult> AuthorizeAsync(
        CommandRequest request,
        CancellationToken cancellationToken = default
    );
}
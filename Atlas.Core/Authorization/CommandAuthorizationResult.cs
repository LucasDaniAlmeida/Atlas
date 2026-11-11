namespace Atlas.Core.Authorization;

public sealed record CommandAuthorizationResult(
    bool IsAuthorized,
    CommandRiskLevel RiskLevel,
    string Message
);
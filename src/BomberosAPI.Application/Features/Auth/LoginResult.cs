namespace BomberosAPI.Application.Features.Auth;

public record LoginResult(
    string Token,
    DateTime ExpiresAt,
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    IReadOnlyList<string> Roles
);

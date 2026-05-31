namespace BomberosAPI.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string Email { get; }
    string FirstName { get; }
    string LastName { get; }
    IReadOnlyList<string> Roles { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}

using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Application.Features.Auth;

public interface IAuthRepository
{
    Task<User?> FindUserByEmailAsync(string email, CancellationToken ct = default);
    Task<UserCredential?> FindCredentialByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<IReadOnlyList<string>> GetActiveRoleCodesByUserIdAsync(Guid userId, CancellationToken ct = default);
}

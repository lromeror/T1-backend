using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Domain.Repositories;

/// <summary>
/// Repository for system users.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
    Task<IEnumerable<User>> GetByRoleAsync(string roleCode, CancellationToken ct = default);
}

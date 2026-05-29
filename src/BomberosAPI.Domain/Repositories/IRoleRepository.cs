using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Domain.Repositories;

/// <summary>
/// Repository for system roles (admin, health staff, trainee, etc.).
/// </summary>
public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}
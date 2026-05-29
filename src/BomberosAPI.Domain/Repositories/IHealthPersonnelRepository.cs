using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Domain.Repositories;

/// <summary>
/// Repository for health personnel (doctors, nurses, etc.).
/// </summary>
public interface IHealthPersonnelRepository : IRepository<HealthPersonnel>
{
   
    /// Finds health personnel by their associated user.
    Task<HealthPersonnel?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// Lists the health personnel authorized to approve medical clearances.
    Task<IEnumerable<HealthPersonnel>> GetWhoCanApproveAsync(CancellationToken cancellationToken = default);
}
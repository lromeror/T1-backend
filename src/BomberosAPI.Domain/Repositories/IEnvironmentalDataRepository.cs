using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Domain.Repositories;

public interface IEnvironmentalDataRepository : IRepository<EnvironmentalData>
{
    Task<IEnumerable<EnvironmentalData>> GetBySessionAsync(Guid trainingSessionId, CancellationToken cancellationToken = default);
}
using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Domain.Repositories;

public interface ITrainingLocationRepository : IRepository<TrainingLocation>
{
    Task<IEnumerable<TrainingLocation>> GetByInstitutionAsync(Guid institutionId, CancellationToken ct = default);
}

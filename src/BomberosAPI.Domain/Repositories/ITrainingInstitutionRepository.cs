using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Domain.Repositories;

/// <summary>
/// Repository for training institutions.
/// </summary>
public interface ITrainingInstitutionRepository : IRepository<TrainingInstitution>
{

    /// Lists only the active institutions.
    Task<IEnumerable<TrainingInstitution>> GetActiveAsync(CancellationToken cancellationToken = default);
}
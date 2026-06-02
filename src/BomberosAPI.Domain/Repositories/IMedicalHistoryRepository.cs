using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Domain.Repositories;

public interface IMedicalHistoryRepository : IRepository<MedicalHistory>
{
    Task<MedicalHistory?> GetByTraineeAsync(Guid traineeFirefighterId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByTraineeAsync(Guid traineeFirefighterId, CancellationToken cancellationToken = default);
}
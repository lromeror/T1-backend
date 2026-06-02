using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Domain.Repositories;

/// <summary>
/// Repository for trainee firefighters.
/// </summary>
public interface ITraineeFirefighterRepository : IRepository<TraineeFirefighter>
{
    /// Finds a trainee by their applicant code.

    Task<TraineeFirefighter?> GetByApplicantCodeAsync(string applicantCode, CancellationToken cancellationToken = default);

    Task<bool> ExistsByApplicantCodeAsync(string applicantCode, CancellationToken cancellationToken = default);


    /// Finds a trainee by their associated user.
    Task<TraineeFirefighter?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
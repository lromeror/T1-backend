using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Domain.Repositories;

public interface IVitalSignsMeasurementRepository : IRepository<VitalSignsMeasurement>
{
    Task<IEnumerable<VitalSignsMeasurement>> GetByParticipantAsync(Guid sessionParticipantId, CancellationToken cancellationToken = default);

    /// Returns all measurements for a trainee (joined through SessionParticipant).
    Task<IEnumerable<VitalSignsMeasurement>> GetByTraineeAsync(Guid traineeFirefighterId, CancellationToken cancellationToken = default);
}
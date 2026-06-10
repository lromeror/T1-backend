using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Domain.Repositories;

public interface IVitalSignsMeasurementRepository : IRepository<VitalSignsMeasurement>
{
    Task<IEnumerable<VitalSignsMeasurement>> GetByParticipantAsync(Guid sessionParticipantId, CancellationToken cancellationToken = default);
}
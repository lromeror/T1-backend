using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using BomberosAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BomberosAPI.Infrastructure.Repositories;

public class VitalSignsMeasurementRepository : IVitalSignsMeasurementRepository
{
    private readonly AppDbContext _db;

    public VitalSignsMeasurementRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<VitalSignsMeasurement>> GetAllAsync(CancellationToken ct = default) =>
        await _db.VitalSignsMeasurements.AsNoTracking().ToListAsync(ct);

    public Task<VitalSignsMeasurement?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _db.VitalSignsMeasurements.FirstOrDefaultAsync(v => v.VitalSignsMeasurementId == id, ct);

    public async Task<IEnumerable<VitalSignsMeasurement>> GetByParticipantAsync(Guid sessionParticipantId, CancellationToken ct = default) =>
        await _db.VitalSignsMeasurements
            .AsNoTracking()
            .Where(v => v.SessionParticipantId == sessionParticipantId)
            .ToListAsync(ct);

    public async Task<IEnumerable<VitalSignsMeasurement>> GetByTraineeAsync(Guid traineeFirefighterId, CancellationToken ct = default) =>
        await (from vs in _db.VitalSignsMeasurements
               join sp in _db.SessionParticipants on vs.SessionParticipantId equals sp.SessionParticipantId
               where sp.TraineeFirefighterId == traineeFirefighterId
               select vs)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task AddAsync(VitalSignsMeasurement measurement, CancellationToken ct = default)
    {
        _db.VitalSignsMeasurements.Add(measurement);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(VitalSignsMeasurement measurement, CancellationToken ct = default)
    {
        _db.VitalSignsMeasurements.Update(measurement);
        await _db.SaveChangesAsync(ct);
    }

    public void Update(VitalSignsMeasurement measurement) => _db.VitalSignsMeasurements.Update(measurement);

    public void Delete(VitalSignsMeasurement measurement) => _db.VitalSignsMeasurements.Remove(measurement);
}
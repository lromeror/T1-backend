using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using BomberosAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BomberosAPI.Infrastructure.Repositories;

public class SessionParticipantRepository : ISessionParticipantRepository
{
    private readonly AppDbContext _db;

    public SessionParticipantRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<SessionParticipant>> GetAllAsync(CancellationToken ct = default) =>
        await _db.SessionParticipants.AsNoTracking().ToListAsync(ct);

    public Task<SessionParticipant?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _db.SessionParticipants.FirstOrDefaultAsync(p => p.SessionParticipantId == id, ct);

    public async Task<IEnumerable<SessionParticipant>> GetBySessionAsync(Guid sessionId, CancellationToken ct = default) =>
        await _db.SessionParticipants
            .AsNoTracking()
            .Where(p => p.TrainingSessionId == sessionId)
            .ToListAsync(ct);

    public async Task<IEnumerable<SessionParticipant>> GetByTraineeAsync(Guid traineeId, CancellationToken ct = default) =>
        await _db.SessionParticipants
            .AsNoTracking()
            .Where(p => p.TraineeFirefighterId == traineeId)
            .ToListAsync(ct);

    public async Task AddAsync(SessionParticipant participant, CancellationToken ct = default)
    {
        _db.SessionParticipants.Add(participant);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(SessionParticipant participant, CancellationToken ct = default)
    {
        _db.SessionParticipants.Update(participant);
        await _db.SaveChangesAsync(ct);
    }

    public void Update(SessionParticipant participant) => _db.SessionParticipants.Update(participant);

    public void Delete(SessionParticipant participant) => _db.SessionParticipants.Remove(participant);
}
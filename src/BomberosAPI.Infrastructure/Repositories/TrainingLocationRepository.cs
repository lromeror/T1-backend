using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using BomberosAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BomberosAPI.Infrastructure.Repositories;

public class TrainingLocationRepository : ITrainingLocationRepository
{
    private readonly AppDbContext _db;

    public TrainingLocationRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<TrainingLocation>> GetAllAsync(CancellationToken ct = default) =>
        await _db.TrainingLocations.AsNoTracking().ToListAsync(ct);

    public Task<TrainingLocation?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _db.TrainingLocations.FirstOrDefaultAsync(l => l.TrainingLocationId == id, ct);

    public async Task<IEnumerable<TrainingLocation>> GetByInstitutionAsync(Guid institutionId, CancellationToken ct = default) =>
        await _db.TrainingLocations.AsNoTracking()
            .Where(l => l.InstitutionId == institutionId)
            .ToListAsync(ct);

    public async Task AddAsync(TrainingLocation entity, CancellationToken ct = default)
    {
        _db.TrainingLocations.Add(entity);
        await _db.SaveChangesAsync(ct);
    }

    public void Update(TrainingLocation entity) => _db.TrainingLocations.Update(entity);

    public async Task UpdateAsync(TrainingLocation entity, CancellationToken ct = default)
    {
        _db.TrainingLocations.Update(entity);
        await _db.SaveChangesAsync(ct);
    }

    public void Delete(TrainingLocation entity) => _db.TrainingLocations.Remove(entity);
}

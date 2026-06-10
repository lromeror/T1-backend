using BomberosAPI.Domain.Repositories;
using BomberosAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EnvironmentalDataEntity = BomberosAPI.Domain.Entities.EnvironmentalData;

namespace BomberosAPI.Infrastructure.Repositories;

public class EnvironmentalDataRepository : IEnvironmentalDataRepository
{
    private readonly AppDbContext _db;

    public EnvironmentalDataRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<EnvironmentalDataEntity>> GetAllAsync(CancellationToken ct = default) =>
        await _db.EnvironmentalData.AsNoTracking().ToListAsync(ct);

    public Task<EnvironmentalDataEntity?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _db.EnvironmentalData.FirstOrDefaultAsync(e => e.EnvironmentalDataId == id, ct);

    public async Task<IEnumerable<EnvironmentalDataEntity>> GetBySessionAsync(Guid trainingSessionId, CancellationToken ct = default) =>
        await _db.EnvironmentalData
            .AsNoTracking()
            .Where(e => e.TrainingSessionId == trainingSessionId)
            .ToListAsync(ct);

    public async Task AddAsync(EnvironmentalDataEntity data, CancellationToken ct = default)
    {
        _db.EnvironmentalData.Add(data);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(EnvironmentalDataEntity data, CancellationToken ct = default)
    {
        _db.EnvironmentalData.Update(data);
        await _db.SaveChangesAsync(ct);
    }

    public void Update(EnvironmentalDataEntity data) => _db.EnvironmentalData.Update(data);

    public void Delete(EnvironmentalDataEntity data) => _db.EnvironmentalData.Remove(data);
}
using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using BomberosAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BomberosAPI.Infrastructure.Repositories;

public class HealthPersonnelRepository : IHealthPersonnelRepository
{
    private readonly AppDbContext _db;

    public HealthPersonnelRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<HealthPersonnel>> GetAllAsync(CancellationToken ct = default) =>
        await _db.HealthPersonnel.AsNoTracking().ToListAsync(ct);

    public Task<HealthPersonnel?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _db.HealthPersonnel.FirstOrDefaultAsync(h => h.HealthPersonnelId == id, ct);

    public Task<HealthPersonnel?> GetByUserIdAsync(Guid userId, CancellationToken ct = default) =>
        _db.HealthPersonnel.FirstOrDefaultAsync(h => h.UserId == userId, ct);

    public async Task<IEnumerable<HealthPersonnel>> GetWhoCanApproveAsync(CancellationToken ct = default) =>
        await _db.HealthPersonnel
            .AsNoTracking()
            .Where(h => h.CanApproveDischarges == true)
            .ToListAsync(ct);

    public async Task AddAsync(HealthPersonnel personnel, CancellationToken ct = default)
    {
        _db.HealthPersonnel.Add(personnel);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(HealthPersonnel personnel, CancellationToken ct = default)
    {
        _db.HealthPersonnel.Update(personnel);
        await _db.SaveChangesAsync(ct);
    }

    public void Update(HealthPersonnel personnel) => _db.HealthPersonnel.Update(personnel);

    public void Delete(HealthPersonnel personnel) => _db.HealthPersonnel.Remove(personnel);
}
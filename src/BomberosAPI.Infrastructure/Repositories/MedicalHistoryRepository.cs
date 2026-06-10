using BomberosAPI.Domain.Repositories;
using BomberosAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MedicalHistoryEntity = BomberosAPI.Domain.Entities.MedicalHistory;

namespace BomberosAPI.Infrastructure.Repositories;

public class MedicalHistoryRepository : IMedicalHistoryRepository
{
    private readonly AppDbContext _db;

    public MedicalHistoryRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<MedicalHistoryEntity>> GetAllAsync(CancellationToken ct = default) =>
        await _db.MedicalHistories.AsNoTracking().ToListAsync(ct);

    public Task<MedicalHistoryEntity?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _db.MedicalHistories.FirstOrDefaultAsync(m => m.MedicalHistoryId == id, ct);

    public Task<MedicalHistoryEntity?> GetByTraineeAsync(Guid traineeFirefighterId, CancellationToken ct = default) =>
        _db.MedicalHistories.FirstOrDefaultAsync(m => m.TraineeFirefighterId == traineeFirefighterId, ct);

    public Task<bool> ExistsByTraineeAsync(Guid traineeFirefighterId, CancellationToken ct = default) =>
        _db.MedicalHistories.AnyAsync(m => m.TraineeFirefighterId == traineeFirefighterId, ct);

    public async Task AddAsync(MedicalHistoryEntity history, CancellationToken ct = default)
    {
        _db.MedicalHistories.Add(history);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(MedicalHistoryEntity history, CancellationToken ct = default)
    {
        _db.MedicalHistories.Update(history);
        await _db.SaveChangesAsync(ct);
    }

    public void Update(MedicalHistoryEntity history) => _db.MedicalHistories.Update(history);

    public void Delete(MedicalHistoryEntity history) => _db.MedicalHistories.Remove(history);
}
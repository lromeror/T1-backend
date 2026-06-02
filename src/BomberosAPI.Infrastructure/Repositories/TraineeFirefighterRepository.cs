using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using BomberosAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BomberosAPI.Infrastructure.Repositories;

public class TraineeFirefighterRepository : ITraineeFirefighterRepository
{
    private readonly AppDbContext _db;

    public TraineeFirefighterRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<TraineeFirefighter>> GetAllAsync(CancellationToken ct = default) =>
        await _db.TraineeFirefighters.AsNoTracking().ToListAsync(ct);

    public Task<TraineeFirefighter?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _db.TraineeFirefighters.FirstOrDefaultAsync(t => t.TraineeFirefighterId == id, ct);

    public Task<TraineeFirefighter?> GetByApplicantCodeAsync(string applicantCode, CancellationToken ct = default) =>
        _db.TraineeFirefighters.FirstOrDefaultAsync(t => t.ApplicantCode == applicantCode, ct);

    public Task<TraineeFirefighter?> GetByUserIdAsync(Guid userId, CancellationToken ct = default) =>
        _db.TraineeFirefighters.FirstOrDefaultAsync(t => t.UserId == userId, ct);

    public Task<bool> ExistsByApplicantCodeAsync(string applicantCode, CancellationToken ct = default) =>
        _db.TraineeFirefighters.AnyAsync(t => t.ApplicantCode == applicantCode, ct);


    public async Task AddAsync(TraineeFirefighter trainee, CancellationToken ct = default)
    {
        _db.TraineeFirefighters.Add(trainee);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(TraineeFirefighter trainee, CancellationToken ct = default)
    {
        _db.TraineeFirefighters.Update(trainee);
        await _db.SaveChangesAsync(ct);
    }

    public void Update(TraineeFirefighter trainee) => _db.TraineeFirefighters.Update(trainee);

    public void Delete(TraineeFirefighter trainee) => _db.TraineeFirefighters.Remove(trainee);
}
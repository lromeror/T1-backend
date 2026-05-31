using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using BomberosAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BomberosAPI.Infrastructure.Repositories;

public class TrainingInstitutionRepository : ITrainingInstitutionRepository
{
    private readonly AppDbContext _db;

    public TrainingInstitutionRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<TrainingInstitution>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _db.TrainingInstitutions.AsNoTracking().ToListAsync(cancellationToken);

    public Task<TrainingInstitution?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _db.TrainingInstitutions.FirstOrDefaultAsync(i => i.InstitutionId == id, cancellationToken);

    public async Task<IEnumerable<TrainingInstitution>> GetActiveAsync(CancellationToken cancellationToken = default) =>
        await _db.TrainingInstitutions.AsNoTracking().Where(i => i.IsActive).ToListAsync(cancellationToken);

    public async Task AddAsync(TrainingInstitution entity, CancellationToken cancellationToken = default)
    {
        _db.TrainingInstitutions.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TrainingInstitution entity, CancellationToken cancellationToken = default)
    {
        _db.TrainingInstitutions.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public void Delete(TrainingInstitution entity) => _db.TrainingInstitutions.Remove(entity);
}

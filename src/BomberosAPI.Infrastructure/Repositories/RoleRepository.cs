using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using BomberosAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BomberosAPI.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _db;

    public RoleRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _db.Roles.AsNoTracking().ToListAsync(cancellationToken);

    public Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _db.Roles.FirstOrDefaultAsync(r => r.RoleId == id, cancellationToken);

    public Task<Role?> GetByCodeAsync(string code, CancellationToken cancellationToken = default) =>
        _db.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Code == code, cancellationToken);

    public async Task AddAsync(Role entity, CancellationToken cancellationToken = default)
    {
        _db.Roles.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Role entity, CancellationToken cancellationToken = default)
    {
        _db.Roles.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public void Delete(Role entity) => _db.Roles.Remove(entity);
}

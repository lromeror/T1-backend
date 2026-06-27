using BomberosAPI.Domain.Entities;
using BomberosAPI.Domain.Repositories;
using BomberosAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BomberosAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Users.AsNoTracking().ToListAsync(ct);

    public Task<User?> GetByIdAsync(Guid userId, CancellationToken ct = default) =>
        _db.Users.FirstOrDefaultAsync(u => u.UserId == userId, ct);

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default) =>
        _db.Users.AnyAsync(u => u.Email == email, ct);

    public async Task<IEnumerable<User>> GetByRoleAsync(string roleCode, CancellationToken ct = default) =>
        await (from u in _db.Users
               join ur in _db.UserRoles on u.UserId equals ur.UserId
               join r  in _db.Roles     on ur.RoleId equals r.RoleId
               where r.Code == roleCode && ur.IsActive
               select u)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
    }

    public void Update(User user) => _db.Users.Update(user);

    public async Task UpdateAsync(User user, CancellationToken ct = default)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync(ct);
    }

    public void Delete(User user) => _db.Users.Remove(user);
}

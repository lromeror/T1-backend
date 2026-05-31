using BomberosAPI.Application.Features.Auth;
using BomberosAPI.Domain.Entities;
using BomberosAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BomberosAPI.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _db;

    public AuthRepository(AppDbContext db) => _db = db;

    public Task<User?> FindUserByEmailAsync(string email, CancellationToken ct = default) =>
        _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<UserCredential?> FindCredentialByUserIdAsync(Guid userId, CancellationToken ct = default) =>
        _db.UserCredentials.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == userId, ct);

    public async Task<IReadOnlyList<string>> GetActiveRoleCodesByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var today = DateTime.UtcNow;
        return await _db.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId
                      && ur.IsActive
                      && ur.StartDate <= today
                      && (ur.EndDate == null || ur.EndDate >= today))
            .Join(_db.Roles, ur => ur.RoleId, r => r.RoleId, (_, r) => r.Code)
            .ToListAsync(ct);
    }

    public async Task UpdateCredentialAsync(UserCredential credential, CancellationToken ct = default)
    {
        _db.UserCredentials.Update(credential);
        await _db.SaveChangesAsync(ct);
    }
}

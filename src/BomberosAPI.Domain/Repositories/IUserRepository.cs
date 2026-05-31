using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Domain.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default);
    Task<User?> GetByIdAsync(Guid userId, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
    Task UpdateAsync(User user, CancellationToken ct = default);
/// <summary>
/// Repository for system users.
/// </summary>
public interface IUserRepository : IRepository<User>
{

    /// Finds a user by email (used in login).
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// Checks whether a user with the given email already exists.
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

    /// Lists the users that belong to a given institution.
    Task<IEnumerable<User>> GetByInstitutionAsync(Guid institutionId, CancellationToken cancellationToken = default);
}
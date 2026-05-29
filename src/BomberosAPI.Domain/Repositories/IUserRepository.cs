using BomberosAPI.Domain.Entities;

namespace BomberosAPI.Domain.Repositories;

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
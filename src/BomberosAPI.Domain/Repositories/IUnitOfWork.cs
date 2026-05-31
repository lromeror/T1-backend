namespace BomberosAPI.Domain.Repositories;

/// <summary>
/// Coordinates the persistence of changes from multiple repositories
/// as a single atomic transaction.
/// </summary>
public interface IUnitOfWork
{
    /// Persists all pending changes in a single transaction.
    /// <returns>Number of affected records.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
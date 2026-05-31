namespace BomberosAPI.Domain.Repositories;

/// <summary>
/// Base contract that any repository in the system must fulfill.
/// Defines the common CRUD operations for any entity.
/// </summary>
/// <typeparam name="T">The entity type handled by this repository.</typeparam>
public interface IRepository<T> where T : class
{

    /// Finds an entity by its unique identifier.
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// Retrieves all entities of type T.
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// Adds a new entity to the context.
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// Marks an entity as modified and persists the change.
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// Marks an entity for deletion.
    void Delete(T entity);
}
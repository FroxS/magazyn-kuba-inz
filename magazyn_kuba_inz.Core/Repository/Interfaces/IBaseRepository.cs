namespace magazyn_kuba_inz.Core.Repository.Interfaces;

public interface IBaseRepository<T> where T : class
{
    /// <summary>
    /// Delete Entity by Id
    /// </summary>
    /// <param name="id">Id of this entity</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Method to gel all entities
    /// </summary>
    /// <returns></returns>
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Method to get Entity by Id
    /// </summary>
    /// <param name="id">Id of entity</param>
    /// <returns></returns>
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Method to insert entity to database
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task InsertAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Method to add entity to database
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Method to save database
    /// </summary>
    /// <returns></returns>
    Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Method to update database
    /// </summary>
    /// <param name="entity">Entity to update</param>
    void Update(T entity);
}
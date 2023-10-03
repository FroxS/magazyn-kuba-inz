namespace magazyn_kuba_inz.Core.Repository.Interfaces;

public interface IBaseRepository<T> where T : class
{
    /// <summary>
    /// Async delete Entity by Id
    /// </summary>
    /// <param name="id">Id of this entity</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Delete Entity by Id
    /// </summary>
    /// <param name="id">Id of this entity</param>
    /// <returns></returns>
    void Delete(Guid id);

    /// <summary>
    /// Async Method to get all entities
    /// </summary>
    /// <returns></returns>
    Task<List<T>> GetAllAsync(bool sortbylp = true, CancellationToken cancellationToken = default(CancellationToken));

    Task<List<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> include, bool sortbylp = true, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Method to get all entities
    /// </summary>
    /// <returns></returns>
    List<T> GetAll(bool sortbylp = true);

    /// <summary>
    /// Async Method to get Entity by Id
    /// </summary>
    /// <param name="id">Id of entity</param>
    /// <returns></returns>
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

    Task<T?> GetByIdAsync(Func<IQueryable<T>, IQueryable<T>> include, Guid id, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Method to get Entity by Id
    /// </summary>
    /// <param name="id">Id of entity</param>
    /// <returns></returns>
    T GetById(Guid id);

    /// <summary>
    /// Async method to add entity to database
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

    void Insert(T entity);

    /// <summary>
    /// Method to add entity to database
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    T Add(T entity);

    /// <summary>
    /// Async method to save database
    /// </summary>
    /// <returns></returns>
    Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Method to save database
    /// </summary>
    /// <returns></returns>
    void Save();

    /// <summary>
    /// Method to update database
    /// </summary>
    /// <param name="entity">Entity to update</param>
    void Update(T entity);

    void ReloadContext();
}
using Warehouse.Models;

namespace Warehouse.Core.Interface;


public interface IBaseService<Model> where Model : BaseEntity
{
    /// <summary>
    /// Delete Entity by Id
    /// </summary>
    /// <param name="id">Id of this entity</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

    bool Delete(Guid id);

    /// <summary>
    /// Method to gel all entities
    /// </summary>
    /// <returns></returns>
    Task<List<Model>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

    List<Model> GetAll();

    /// <summary>
    /// Method to get Entity by Id
    /// </summary>
    /// <param name="id">Id of entity</param>
    /// <returns></returns>
    Task<Model> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Method to add entity to database
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AddAsync(Model entity, CancellationToken cancellationToken = default(CancellationToken));

    bool Add(Model entity);

    /// <summary>
    /// Method to update database
    /// </summary>
    /// <param name="entity">Entity to update</param>
    void Update(Model entity);

    Task<bool> SaveAsync(CancellationToken cancellationToken = default(CancellationToken));

    bool Save();

    void RefreshDbContext();

    bool Exist(Guid id);

    Model GetById(Guid id);
    void EndTransaction();
    void RunTransaction();

    IApp GetApp();
}
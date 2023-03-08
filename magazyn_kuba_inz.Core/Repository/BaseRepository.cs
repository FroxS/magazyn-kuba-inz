using magazyn_kuba_inz.Core.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace magazyn_kuba_inz.Core.Repository;

public abstract class BaseRepository<T, C> : IBaseRepository<T> where T : class where C : DbContext
{
    #region Protected properties

    /// <summary>
    /// Context of database
    /// </summary>
    protected readonly C context;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="context">Context od database</param>
    public BaseRepository(C context)
    {
        this.context = context;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Method to get all entites to list
    /// </summary>
    /// <returns></returns>
    public async virtual Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        return await context.Set<T>().ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Method to get one Tenity from databae by Id
    /// </summary>
    /// <param name="id">Id of this Entity</param>
    /// <returns></returns>
    public async virtual Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await context.Set<T>().FindAsync(id, cancellationToken);
    }

    /// <summary>
    /// Method to insert entity to database
    /// </summary>
    /// <param name="task">Created entiti to past to the database</param>
    /// <returns></returns>
    public async virtual Task InsertAsync(T task, CancellationToken cancellationToken = default(CancellationToken))
    {
        await context.Set<T>().AddAsync(task,cancellationToken);
    }

    /// <summary>
    /// Method to delete entity from database
    /// </summary>
    /// <param name="id">Id of this entity</param>
    /// <returns></returns>
    public async virtual Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
    {
        T task = await context.Set<T>().FindAsync(id, cancellationToken);
        context.Set<T>().Remove(task);
    }

    /// <summary>
    /// Method to delete entity from database
    /// </summary>
    /// <param name="id">Id of this entity</param>
    /// <returns></returns>
    public async virtual Task AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
    {
        await context.Set<T>().AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
    }

    /// <summary>
    /// Method to update entity in database
    /// </summary>
    /// <param name="entity">Entity to update</param>
    public virtual void Update(T entity)
    {
        context.Entry(entity).State = EntityState.Modified;
    }

    /// <summary>
    /// Method to save change in database
    /// </summary>
    /// <returns></returns>
    public async Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    #endregion
}
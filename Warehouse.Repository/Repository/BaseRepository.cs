﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Threading;
using System.Windows.Controls;
using Warehouse.Core.Interface;
using Warehouse.EF.Migrations;
using Warehouse.Models;
using Warehouse.Repository.Interfaces;

namespace Warehouse.Repository;

internal abstract class BaseRepository<T, C> : IBaseRepository<T> where T : BaseEntity where C : DbContext
{
    #region Protected properties

    /// <summary>
    /// Context of database
    /// </summary>
    protected C _context => (C)_app.Database;

    protected readonly IApp _app;

    public IDbContextTransaction? ActualTransaction => _context.Database.CurrentTransaction;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="context">Context od database</param>
    public BaseRepository(IApp app)
    {
        _app = app;
    }

    #endregion

    #region Protected methods

    protected virtual IQueryable<T> Order(IQueryable<T> items, bool sortbylp = true)
    {
        if (sortbylp)
            return items.OrderBy(x => x.Lp);
        else
            return items.OrderBy(x => x.ID);
    }

    #endregion

    #region Public methods

    public void ReloadContext()
    {
        _app.ReloadDatabase();
    }

    public void RunTransaction()
    {
        //_context.Database.BeginTransaction();
    }

    public void EndTransaction()
    {
        //_context.Database.RollbackTransaction();
    }


    public IQueryable<T> GetItemsInclude(Func<IQueryable<T>, IQueryable<T>> include)
    {
        IQueryable<T> items = _context.Set<T>();
        return include.Invoke(items);
    }

    /// <summary>
    /// Async method to get all entites to list
    /// </summary>
    /// <returns></returns>
    public async virtual Task<List<T>> GetAllAsync(bool sortbylp = true, CancellationToken cancellationToken = default(CancellationToken))
    {
        IQueryable<T> items = _context.Set<T>();
        return await Order(items, sortbylp).ToListAsync(cancellationToken);
    }


    public async Task<List<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> include, bool sortbylp = true, CancellationToken cancellationToken = default(CancellationToken))
    {
        IQueryable<T> items = _context.Set<T>();
        items = include.Invoke(items);
        return await Order(items, sortbylp).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Method to get all entites to list
    /// </summary>
    /// <returns></returns>
    public virtual List<T> GetAll(bool sortbylp = true)
    {
        IQueryable<T> items = _context.Set<T>();
        return Order(items, sortbylp).ToList();
    }

    /// <summary>
    /// Method to get all entites to list
    /// </summary>
    /// <returns></returns>
    public virtual List<T> GetAll(Func<IQueryable<T>, IQueryable<T>> include, bool sortbylp = true)
    {
        IQueryable<T> items = _context.Set<T>();
        items = include.Invoke(items);
        return Order(items, sortbylp).ToList();
    }

    /// <summary>
    /// Async method to get one Tenity from databae by Id
    /// </summary>
    /// <param name="id">Id of this Entity</param>
    /// <returns></returns>
    public async virtual Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken)) => await _context.Set<T>().FindAsync(id, cancellationToken);

    public async Task<T?> GetByIdAsync(Func<IQueryable<T>, IQueryable<T>> include,Guid id, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await GetItemsInclude(include).FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
    }

    public T? GetById(Func<IQueryable<T>, IQueryable<T>> include, Guid id)
    {

        //T item = _context.Set<T>().Find(id);

        //_context.Entry(item);
        return GetItemsInclude(include).FirstOrDefault(x => x.ID == id);

    }


    /// <summary>
    /// Method to get one Tenity from databae by Id
    /// </summary>
    /// <param name="id">Id of this Entity</param>
    /// <returns></returns>
    public virtual T GetById(Guid id) => _context.Set<T>().Find(id);



    /// <summary>
    /// Method to get one Tenity from databae by Id
    /// </summary>
    /// <param name="id">Id of this Entity</param>
    /// <returns></returns>
    public virtual bool Exist(Guid id) => GetById(id) != null;

    /// <summary>
    /// Async method to delete entity from database
    /// </summary>
    /// <param name="id">Id of this entity</param>
    /// <returns></returns>
    public async virtual Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
    {
        T task = await _context.Set<T>().FindAsync(id, cancellationToken);
        _context.Set<T>().Remove(task);
    }

    /// <summary>
    /// Method to delete entity from database
    /// </summary>
    /// <param name="id">Id of this entity</param>
    /// <returns></returns>
    public virtual void Delete(Guid id)
    {
        T? task = _context.Set<T>().Find(id);
        _context.Set<T>().Remove(task);
    }

    /// <summary>
    /// Method to delete entity from database
    /// </summary>
    /// <param name="id">Id of this entity</param>
    /// <returns></returns>
    //public async virtual Task<T> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken)) => (await _context.Set<T>().AddAsync(entity, cancellationToken)).Entity;
    public async virtual Task<T> AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
    {
        var ent = _context.Set<T>();
        entity.CreatedAt = entity.Modified = DateTime.Now;
        var res = await ent.AddAsync(entity, cancellationToken);

        return res.Entity;


		//(await _context.Set<T>().AddAsync(entity, cancellationToken)).Entity;
    }

	/// <summary>
	/// Method to delete entity from database
	/// </summary>
	/// <param name="id">Id of this entity</param>
	/// <returns></returns>
	public virtual void Insert(T entity)
    {
        entity.CreatedAt = entity.Modified = DateTime.Now;
        _context.Entry(entity).State = EntityState.Added;
    }


    /// <summary>
    /// Async method to delete entity from database
    /// </summary>
    /// <param name="id">Id of this entity</param>
    /// <returns></returns>
    public virtual T Add(T entity)
    {
        entity.CreatedAt = entity.Modified = DateTime.Now;
        return _context.Set<T>().Add(entity).Entity;
    }

    /// <summary>
    /// Method to update entity in database
    /// </summary>
    /// <param name="entity">Entity to update</param>
    public virtual void Update(T entity)
    {
        entity.Modified = DateTime.Now;
        _context.Set<T>().Update(entity);
       // _context.Entry(entity).State = EntityState.Modified;
    }

    /// <summary>
    /// Async method to update entity in database
    /// </summary>
    /// <param name="entity">Entity to update</param>
    public virtual async Task RefteshDataAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _context.Entry<T>(entity).ReloadAsync(cancellationToken);
    }

    /// <summary>
    /// Async method to save change in database
    /// </summary>
    /// <returns></returns>
    public async Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Method to save change in database
    /// </summary>
    /// <returns></returns>
    public void Save()
    {
        _context.SaveChanges();
    }

    #endregion

}
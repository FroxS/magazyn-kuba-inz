﻿using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Models.WareHouse;

namespace magazyn_kuba_inz.Core.Service;

public class BaseServiceWithRepository<R, M> : BaseService<M> where M : BaseEntity where R : IBaseRepository<M>
{

    #region Private fields

    protected new readonly R _repozitory;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public BaseServiceWithRepository(R repozitory) : base(repozitory)
    {
        _repozitory = repozitory;
    }
    #endregion
}

public class BaseService<Model>: IBaseService<Model> where Model : class
{
    #region protected fields

    protected readonly IBaseRepository<Model> _repozitory;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public BaseService(IBaseRepository<Model> repozitory)
    {
        _repozitory = repozitory;
    }

    #endregion

    #region Public Method

    public virtual async Task<bool> AddAsync(Model model, bool saveData = true)
    {
        try
        {
            await _repozitory.AddAsync(model);
            if(saveData)
                await _repozitory.SaveAsync();
            return true;
        }
        catch (Exception ex) { return false; }
    }

    public virtual void Update(Model model)
    {
        try
        {
            _repozitory.Update(model);
        }
        catch (Exception ex) {  }
    }

    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _repozitory.DeleteAsync(id);
        return true;
    }

    public virtual bool Delete(Guid id)
    {
        _repozitory.Delete(id);
        return true;
    }

    public virtual async Task<List<Model>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        
        return await _repozitory.GetAllAsync(); 
        
    }

    public virtual List<Model> GetAll()
    {
        
        return _repozitory.GetAll();
    }

    public virtual async Task<Model> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repozitory.GetByIdAsync(id);
    }

    public virtual async Task<bool> AddAsync(Model entity, CancellationToken cancellationToken = default)
    {
        await _repozitory.AddAsync(entity);
        //await _repozitory.SaveAsync();
        return true;
    }

    public virtual async Task<bool> SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        await _repozitory.SaveAsync(cancellationToken);
        return true;
    }

    public virtual bool Save()
    {
        _repozitory.Save();
        return true;
    }

    public void RefreshDbContext()
    {
        _repozitory.ReloadContext();
    }

    #endregion
}

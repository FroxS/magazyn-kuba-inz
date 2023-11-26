using Warehouse.Repository.Interfaces;
using Warehouse.Models;
using Warehouse.Core.Interface;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json;
using Warehouse.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Service;

internal class BaseServiceWithRepository<R, M> : BaseService<M> where M : BaseEntity where R : IBaseRepository<M>
{

    #region Private fields

    protected new readonly R _repozitory;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public BaseServiceWithRepository(R repozitory, IApp app) : base(repozitory, app)
    {
        _repozitory = repozitory;
    }
    #endregion
}

internal class BaseService<Model>: IBaseService<Model> where Model : BaseEntity
{
    #region protected fields

    protected readonly IBaseRepository<Model> _repozitory;

    protected readonly IApp _app;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public BaseService(IBaseRepository<Model> repozitory, IApp app)
    {
        _repozitory = repozitory;
        _app = app;
    }

    #endregion

    #region Helper Method

    public IApp GetApp() => _app;

    protected virtual byte[] GetData<T>(T obj, Action<JsonSerializer> onSerialize = null)
    {
        MemoryStream ms = new MemoryStream();
        using (BsonDataWriter writer = new BsonDataWriter(ms))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented; //Format the output
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            onSerialize?.Invoke(serializer);
            serializer.Serialize(writer, obj);
        }
        return ms.ToArray();
    }

    protected virtual T GetData<T>(byte[] data)
    {
        T obj = default(T);
        using(MemoryStream ms = new MemoryStream(data))
        {
            using (BsonDataReader reader = new BsonDataReader(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                obj = serializer.Deserialize<T>(reader);
            }
        }
        return obj;
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
        }catch(Exception ex)
        {
            _app.CatchExeption(ex);
        }
        
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

    public virtual async Task<List<Model>> GetAllAsync(CancellationToken cancellationToken = default) => await _repozitory.GetAllAsync(cancellationToken: cancellationToken);

    public virtual List<Model> GetAll() => _repozitory.GetAll();

    public virtual async Task<Model> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => await _repozitory.GetByIdAsync(id);

    public virtual Model GetById(Guid id) => _repozitory.GetById(id);

    public virtual async Task<bool> AddAsync(Model entity, CancellationToken cancellationToken = default)
    {
        await _repozitory.AddAsync(entity);
        return true;
    }

    public virtual bool Add(Model entity) => _repozitory.Add(entity) != null;

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

    public virtual bool Exist(Guid id) => _repozitory.Exist(id);

    public void RefreshDbContext()
    {
        _repozitory.ReloadContext();
    }

    public void RunTransaction()
    {
        _repozitory.RunTransaction();
    }

    public void EndTransaction()
    {
        _repozitory.EndTransaction();
    }

    public void InTransact(Action<IBaseRepository<Model>> actionInTransact)
    {
        RunTransaction();
        actionInTransact.Invoke(_repozitory);
        EndTransaction();
    }

    #endregion
}

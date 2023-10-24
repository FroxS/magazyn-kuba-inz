using Warehouse.Repository.Interfaces;
using Warehouse.Service.Interface;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Service;

public class RackService : BaseServiceWithRepository<IRackRepository, Rack>, IRackService
{
    #region Private fields

    private readonly IHallRepository _hallRepository;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public RackService(IRackRepository repository, IHallRepository hallRepository) :base(repository)
    {
        _hallRepository = hallRepository;
    }

    #endregion

    #region Public Method

    public override bool Delete(Guid id)
    {
        if (!CanDeleteRack(id))
            return false;
        return base.Delete(id);
    }

    public async override Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (!CanDeleteRack(id))
            return false;
        return await base.DeleteAsync(id, cancellationToken);
    }


    public bool CanDeleteRack(Guid id)
    {
        Rack? obj = _repozitory.GetById(o => o.Include(x => x.StorageItems),id);
        if (obj == null)
            return true;
        return (obj.StorageItems?.Count ?? 0) <= 0;
    }

    public List<Rack> GetAllWithItems()
    {
        return _repozitory.GetAll(x => x.Include(i => i.StorageItems).ThenInclude(i => i.Items).ThenInclude(i => i.Product));
    }

    #endregion
}
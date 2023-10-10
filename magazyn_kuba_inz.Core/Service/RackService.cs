using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Models.WareHouse;
using Microsoft.EntityFrameworkCore;

namespace magazyn_kuba_inz.Core.Service;

public class RackService : BaseServiceWithRepository<IRackRepository, Rack>, IRackService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public RackService(IRackRepository ripository) :base(ripository)
    {
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

    #endregion
}
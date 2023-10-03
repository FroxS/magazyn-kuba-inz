using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.EF;
using magazyn_kuba_inz.Models.WareHouse;
using Microsoft.EntityFrameworkCore;

namespace magazyn_kuba_inz.Core.Repository;

public class StorageUnitRepository : BaseRepository<StorageUnit,WarehouseDbContext>, IStorageUnitRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public StorageUnitRepository(IDbContextFactory<WarehouseDbContext> factory) : base(factory)
    {

    }

    #endregion

    #region Public methods

    #endregion
}
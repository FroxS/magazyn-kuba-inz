using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.EF;
using magazyn_kuba_inz.Models.WareHouse;
using Microsoft.EntityFrameworkCore;

namespace magazyn_kuba_inz.Core.Repository;

public class StorageItemCollectionRepository : BaseRepository<StorageItemCollection,WarehouseDbContext>, IStorageItemCollectionRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public StorageItemCollectionRepository(IDbContextFactory<WarehouseDbContext> factory) : base(factory)
    {

    }

    #endregion

    #region Public methods

    #endregion
}
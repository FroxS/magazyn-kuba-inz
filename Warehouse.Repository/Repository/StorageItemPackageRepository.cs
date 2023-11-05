using Warehouse.Repository.Interfaces;
using Warehouse.EF;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Interface;

namespace Warehouse.Repository;

internal class StorageItemPackageRepository : BaseRepository<StorageItemPackage,WarehouseDbContext>, IStorageItemPackageRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public StorageItemPackageRepository(IApp app) : base(app)
    {

    }

    #endregion

    #region Public methods

    #endregion
}
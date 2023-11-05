using Warehouse.Repository.Interfaces;
using Warehouse.EF;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Interface;

namespace Warehouse.Repository;

internal class StorageUnitRepository : BaseRepository<StorageUnit,WarehouseDbContext>, IStorageUnitRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public StorageUnitRepository(IApp app) : base(app)
    {

    }

    #endregion

    #region Public methods

    #endregion
}
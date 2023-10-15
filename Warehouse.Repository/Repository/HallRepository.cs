using Warehouse.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Warehouse.Models;
using Warehouse.EF;

namespace Warehouse.Repository;

internal class HallRepository : BaseRepository<Hall, WarehouseDbContext>, IHallRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public HallRepository(IDbContextFactory<WarehouseDbContext> factory) : base(factory)
    {

    }

    #endregion

    #region Public methods

    #endregion
}
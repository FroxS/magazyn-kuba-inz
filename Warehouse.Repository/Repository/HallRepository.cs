using Warehouse.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Warehouse.Models;
using Warehouse.EF;
using Warehouse.Core.Interface;

namespace Warehouse.Repository;

internal class HallRepository : BaseRepository<Hall, WarehouseDbContext>, IHallRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public HallRepository(IApp app) : base(app)
    {

    }

    #endregion

    #region Public methods

    #endregion
}
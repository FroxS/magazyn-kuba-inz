using Warehouse.Repository.Interfaces;
using Warehouse.EF;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Interface;

namespace Warehouse.Repository;

internal class RackRepository : BaseRepository<Rack,WarehouseDbContext>, IRackRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public RackRepository(IApp app) : base(app)
    {

    }

    #endregion

    #region Public methods

    protected override IQueryable<Rack> Order(IQueryable<Rack> items, bool sortbylp = true)
    {
        if (sortbylp)
            return items.OrderBy(x => x.Lp);
        else
            return items.OrderBy(x => x.Corridor).ThenBy(x => x.Row).ThenBy(x => x.Direction);
    }

    #endregion
}
using Warehouse.Repository.Interfaces;
using Warehouse.EF;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Interface;

namespace Warehouse.Repository;

internal class OrderRepository : BaseRepository<Order,WarehouseDbContext>, IOrderRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public OrderRepository(IApp app) : base(app)
    {

    }

    #endregion

    #region Public methods

    #endregion
}
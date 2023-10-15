using Warehouse.Repository.Interfaces;
using Warehouse.Service.Interface;
using Warehouse.Models;

namespace Warehouse.Service;

public class OrderService : BaseServiceWithRepository<IOrderRepository,Order>, IOrderService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public OrderService(IOrderRepository ripository) :base(ripository)
    {
    }

    #endregion

    #region Public Method

    #endregion
}
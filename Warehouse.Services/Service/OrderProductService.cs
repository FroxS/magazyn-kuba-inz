using Warehouse.Repository.Interfaces;
using Warehouse.Service.Interface;
using Warehouse.Models;

namespace Warehouse.Service;

internal class OrderProductService : BaseServiceWithRepository<IOrderProductRepository,OrderProduct>, IOrderProductService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public OrderProductService(IOrderProductRepository ripository) :base(ripository)
    {
    }

    #endregion

    #region Public Method

    #endregion
}
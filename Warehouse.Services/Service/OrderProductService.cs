using Warehouse.Repository.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.Models;

namespace Warehouse.Service;

internal class OrderProductService : BaseServiceWithRepository<IOrderProductRepository,OrderProduct>, IOrderProductService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public OrderProductService(IOrderProductRepository repozitory, IApp app) : base(repozitory, app)
    {
    }

    #endregion

    #region Public Method

    #endregion
}
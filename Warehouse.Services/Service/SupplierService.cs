using Warehouse.Repository.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.Models;

namespace Warehouse.Service;

internal class SupplierService : BaseServiceWithRepository<ISupplierRepository,Supplier>, ISupplierService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public SupplierService(ISupplierRepository repozitory, IApp app) : base(repozitory, app)
    {
    }

    #endregion

    #region Public Method

    public Supplier Add(Supplier supplier)
    {
        if (supplier == null)
            throw new ArgumentException("Supplier is null");

        _repozitory.Insert(supplier);
        return supplier;  
    }

    #endregion
}
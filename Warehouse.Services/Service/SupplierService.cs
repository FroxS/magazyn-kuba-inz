using Warehouse.Repository.Interfaces;
using Warehouse.Service.Interface;
using Warehouse.Models;

namespace Warehouse.Service;

public class SupplierService : BaseServiceWithRepository<ISupplierRepository,Supplier>, ISupplierService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public SupplierService(ISupplierRepository supplierpozitory) :base(supplierpozitory)
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
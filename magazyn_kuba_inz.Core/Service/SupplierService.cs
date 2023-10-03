using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.Models.WareHouse;
using System.Collections.ObjectModel;

namespace magazyn_kuba_inz.Core.Service;

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
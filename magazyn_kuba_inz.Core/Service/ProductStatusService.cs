using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.Models.WareHouse;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace magazyn_kuba_inz.Core.Service;

public class ProductStatusService : BaseServiceWithRepository<IProductStatusRepository, ProductStatus>, IProductStatusService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public ProductStatusService(IProductStatusRepository repository):base(repository)
    {
        
    }

    #endregion

    #region Public Method

    public async Task<ProductStatus> AddAsync(ProductStatus item)
    {
        if (item == null)
            throw new ArgumentException("Product group is null");

        if ((await GetAllAsync()).Exists(x => x.Name == item.Name))
            throw new ArgumentException($"Artykuł o nazwie {item.Name} już istnieje");

        await _repozitory.AddAsync(item);
        return item;
    }

    #endregion
}
using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.Models.WareHouse;
using System.Collections.ObjectModel;

namespace magazyn_kuba_inz.Core.Service;

public class ProductGroupService : BaseServiceWithRepository<IProductGroupRepository, ProductGroup>, IProductGroupService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public ProductGroupService(IProductGroupRepository productgrouprepozitory):base(productgrouprepozitory)
    {
        
    }

    #endregion

    #region Public Method

    public async Task<ProductGroup> AddAsync(ProductGroup item)
    {
        if (item == null)
            throw new ArgumentException("Product group is null");

        if ((await GetAllAsync()).Exists(x => x.Name == item.Name))
            throw new ArgumentException($"Artykuł o nazwie {item.Name} już istnieje");

        await _repozitory.AddAsync(item);
        return item;
    }

    public async Task<ObservableCollection<ProductGroup>> GetAllProductGroupToViewModel()
    {
        var items = new ObservableCollection<ProductGroup>();
        foreach (var item in await _repozitory.GetAllAsync())
            if (item != null)
                items.Add(item);

        return items;
    }

    #endregion
}
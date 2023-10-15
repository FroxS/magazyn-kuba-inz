using Warehouse.Repository.Interfaces;
using Warehouse.Service.Interface;
using Warehouse.Models;
using System.Collections.ObjectModel;

namespace Warehouse.Service;

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
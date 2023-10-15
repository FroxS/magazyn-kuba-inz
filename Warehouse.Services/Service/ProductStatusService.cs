using Warehouse.Repository.Interfaces;
using Warehouse.Service.Interface;
using Warehouse.Models;
using System.Collections.ObjectModel;
namespace Warehouse.Service;

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
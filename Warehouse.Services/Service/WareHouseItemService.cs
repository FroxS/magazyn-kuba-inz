using Warehouse.Repository.Interfaces;
using Warehouse.Service.Interface;
using Warehouse.Models;

namespace Warehouse.Service;

public class WareHouseItemService : BaseServiceWithRepository<IWareHouseItemRepository, WareHouseItem>, IWareHouseItemService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public WareHouseItemService(IWareHouseItemRepository repository):base(repository)
    {  
    }

    #endregion

    #region Public Method

    public async Task<int> GetCoutOfProduct(Product product)
    {
        return (await _repozitory.GetItemAsync(product.ID)).Select(x => x.Count).Sum();
    }

    public async Task<int> GetCoutOfProduct(Product product, ItemState state)
    {
        return (await _repozitory.GetItemAsync(product.ID, state.ID))?.Count ?? 0;
    }

    public async Task<List<Product?>> GetProductsByState(ItemState state)
    {
        return await _repozitory.GetProdictsWidthStateAsync(state.ID);
    }

    public async Task<bool> ExistProduct(Guid product) => (await _repozitory.GetAllAsync()).Any(x => x.ID_Product == product);

    public async Task<WareHouseItem> RemoveProduct(Product product, ItemState status, CancellationToken token = default(CancellationToken))
    {
        if (product == null)
            return null;

        WareHouseItem? found = await _repozitory.GetItemAsync(product.ID, status.ID, token);
        if (found == null)
            return found;

        return await SetCountProduct(product, status, found.Count - 1, token); ;
    }

    public async Task<WareHouseItem> AddProduct(Product product, ItemState status, CancellationToken token = default(CancellationToken))
    {
        if (product == null || status == null)
            return null;

        WareHouseItem? found = await _repozitory.GetItemAsync(product.ID, status.ID, token);
        if (found == null)
        {
            return await SetCountProduct(product,status,1,token);
        }
        return await SetCountProduct(product, status, found.Count + 1, token); ;
    }

    public async Task<WareHouseItem> SetCountProduct(Product product, ItemState status, int count, CancellationToken token = default(CancellationToken))
    {
        bool flag = false;

        if (product == null || status == null)
            return null;

        WareHouseItem? found = await _repozitory.GetItemAsync(product.ID, status.ID, token);

        if (count <= 0 && found != null)
        {
            _repozitory.Delete(found.ID);
            await _repozitory.SaveAsync(token);
            return null;
        }

        if (found == null)
        {
            found = WareHouseItem.Get();
            found.State = status;
            found.Product = product;
            found.ID_Product = product.ID;
            found.ID_State = status.ID;
            found.Count = count <= 0 ? 1 : count;
            await _repozitory.AddAsync(found, token);
            await _repozitory.SaveAsync(token);
            flag = true;
            return found;
        }
        found.Count = count;
        _repozitory.Update(found);
        await _repozitory.SaveAsync(token);
        flag = true;
        return found;
    }

    public async Task<List<WareHouseItem>> GetAllByProductAsync(Guid productid, CancellationToken token = default(CancellationToken))
    {
        return await _repozitory.GetItemAsync(productid, token);
    }

    #endregion
}
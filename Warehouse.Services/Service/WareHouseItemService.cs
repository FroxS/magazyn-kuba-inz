using Warehouse.Repository.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using Warehouse.Models.Enums;

namespace Warehouse.Service;

internal class WareHouseItemService : BaseServiceWithRepository<IWareHouseItemRepository, WareHouseItem>, IWareHouseItemService
{
    #region Private fields

    private readonly IItemStateRepository _stateRepo;

    private readonly IStorageItemRepository _storageItemRepository;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public WareHouseItemService(IWareHouseItemRepository repository, IItemStateRepository stateRepo, IStorageItemRepository storageItemRepository):base(repository)
    {
        _stateRepo = stateRepo;
        _storageItemRepository = storageItemRepository;
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

    public List<WareHouseItem> GetProductsByState(Guid stateId)
    {
        return _repozitory.GetAll(x => x.Include(i => i.Product)).Where(x => x.ID_State == stateId).OrderBy(x => x.State).ToList();
    }

    public WareHouseItem? GetItem(Guid productId, Guid stateId)
    {
        return _repozitory.GetAll(x => x.Include(i => i.Product).Include(i => i.State)).Where(x => x.ID_State == stateId && x.ID_Product == productId).OrderBy(x => x.State).FirstOrDefault();
    }

    public async Task<bool> ExistProduct(Guid product) => (await _repozitory.GetAllAsync()).Any(x => x.ID_Product == product);

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

    public string MoveProductToState(ref WareHouseItem item, Guid targetState, int count)
    {
        string messages = null;
        WareHouseItem targetitem = GetItem(item.ID_Product, targetState);
        count = item.Count > count ? count : item.Count;
        ItemState targetstate = _stateRepo.GetById(targetState);
        bool added = false;
        if (targetitem == null)
        {
            targetitem = WareHouseItem.Get();
            targetitem.Count = 0;
            targetitem.ID_State = targetState;
            targetitem.ID_Product = item.ID_Product;
            added = true;
        }
        if (CanMove(item.State.State, targetstate.State))
        {
            targetitem.Count += count;
            
            if (added)
                _repozitory.Insert(targetitem);
            else
                _repozitory.Update(targetitem);

            if(item.Count == count)
                _repozitory.Delete(item.ID);
            else
            {
                item.Count -= count;
                _repozitory.Update(item);
            }

            item = targetitem;
        }
        else
            messages = "Nie można przenieść";
        return messages;

    }

    public bool CanMove(EState oldState, EState newState)
    {
        int oldStateValue = (int)oldState;
        int newStateValue = (int)newState;

        if ((oldStateValue & (oldStateValue - 1)) == 0 && oldStateValue < newStateValue)
        {
            // newState jest bezpośrednio poziom wyżej od oldState
            return true;
        }
        else if ((newStateValue & (newStateValue - 1)) == 0 && newStateValue < oldStateValue)
        {
            // newState jest bezpośrednio poziom niżej od oldState
            return true;
        }
        else
        {
            return false; // newState i oldState są na tym samym poziomie lub mają inne stany pośrednie
        }
    }

    public List<WareHouseItem> GetProductsAvailableToRack()
    {
        return _repozitory.GetAll(x => x.Include(i => i.State).Include(i => i.Product)).Where(x => x.State.State == EState.InStock).ToList();
    }

    public bool MoveItemsToRack(IEnumerable<StorageItem> items)
    {
        var possible = GetProductsAvailableToRack();
        foreach(var item in items.GroupBy(x => x.ID_Item))
        {
            var count = possible.Where(x=> x.ID == item.Key).Select(x => x.Count).Sum();
            if (!(item.Count() <= count))
                return false;

        }

        Guid IdState = _stateRepo.GetAll().FirstOrDefault(x => x.State == EState.Available).ID;

        foreach (var item in items.GroupBy(x => x.ID_Item))
        {
            var whitem = GetById(item.Key);
            MoveProductToState(ref whitem, IdState, item.Count());
            foreach (var itemState in item)
            {
                itemState.ID_Item = whitem.ID;
                _storageItemRepository.Add(itemState);
            }
            _storageItemRepository.Save();
            
            
        }

        return true;
    }

    public bool RemoveFromRack(StorageItem item)
    {
        if (item.ID_OrderItem != null)
            return false;

        Guid IdState = _stateRepo.GetAll().FirstOrDefault(x => x.State == EState.InStock).ID;
        var whitem =  GetById(item.ID_Item);
        if(MoveProductToState(ref whitem, IdState, 1) == null)
        {
            _storageItemRepository.Delete(item.ID);
            _storageItemRepository.Save();
            return true;
        }
        return false;
    }

    #endregion
}
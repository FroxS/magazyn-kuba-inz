using Warehouse.Repository.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using Warehouse.Models.Enums;
using System.Linq;

namespace Warehouse.Service;

internal class WareHouseService : IWareHouseService
{
    #region Private fields

    private readonly IStorageItemRepository _storageItemRepo;
    private readonly IItemStateRepository _stateRepo;
    private readonly IStorageItemPackageRepository _storageItemPackageRepo;
    private readonly IOrderProductRepository _orderProductRepo;
    private readonly IStorageItemPackageRepository _pagkageRepo;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public WareHouseService(
        IStorageItemRepository storageItemRepo, 
        IItemStateRepository stateRepo, 
        IStorageItemPackageRepository storageItemPackageRepo, 
        IOrderProductRepository orderProductRepo,
        IStorageItemPackageRepository pagkageRepo)
    {
        _storageItemRepo = storageItemRepo;
        _stateRepo = stateRepo;
        _storageItemPackageRepo = storageItemPackageRepo;
        _orderProductRepo = orderProductRepo;
        _pagkageRepo = pagkageRepo;
    }

    #endregion

    #region Helpers

    private EState GetState(StorageItem item)
    {
        if (item.State == null)
            return _stateRepo.GetById(item.ID_State).State;
        else
            return item.State.State;
    }

    private bool CanMove(EState oldState, EState newState)
    {
        int oldStateValue = (int)oldState;
        int newStateValue = (int)newState;

        if ((newStateValue & (oldStateValue * 2)) == newStateValue)
            return true;
        else if ((oldStateValue & (newStateValue * 2)) == oldStateValue)
        {
            return true;
        }  
        else
            return false;
    }

    #endregion

    #region Public methods

    public bool Add(Guid product, EState state, int count)
    {
        ItemState stateobj = _stateRepo.GetByState(state);
        if (stateobj == null)
            return false;

        if (!CanAddProduct(product, state))
            return false;

        for (int i = 0; i < count; i++)
        {
            StorageItem item = StorageItem.Get();
            item.ID_State = stateobj.ID;
            item.ID_Product = product;
            if (_storageItemRepo.Add(item) == null)
                return false;
        }
        return true;
    }

    public bool CanAddProduct(Guid product, EState state)
    {
        if((state & (EState.Delivery | EState.Received)) != 0)
            return true;
        return false;
    }

    public bool ExistProduct(Guid idProduct) => _storageItemRepo.GetAll().Any(x => x.ID_Product == idProduct);

    public List<StorageItem> GetProductsByState(EState state)
    {
        return _storageItemRepo.GetAll(x => x.Include(i => i.State).Include(i => i.Product))
            .Where(x => (x.State?.State & state) == x.State?.State)
            .ToList();
    }

    private string CheckMovingItem(StorageItem item, EState targetStateType)
    {
        ItemState targetState = _stateRepo.GetByState(targetStateType);
        if (targetStateType >= EState.Available )
        {
            if (item.ID_Package == null || !_storageItemPackageRepo.Exist(item.ID_Package.Value))
                return $"Produkt powinein mieć przypisaną paczkę z przed wrzuceniem go do stanu {targetState.Name}";
        }
        else
        {
            item.ID_Package = null;
            item.Package = null;
        }

        if (targetStateType >= EState.Reserved)
        {
            if (item.ID_OrderItem == null || !_orderProductRepo.Exist(item.ID_OrderItem.Value))
                return $"Produkt powinein mieć przypisany produkt z oferty z stanem {targetState.Name}";
        }
        else
        {
            item.ID_OrderItem = null;
            item.OrderItem = null;
        }
        return null;
    }

    public string? MoveProductToState(EState targetStateType, params StorageItem[] items)
    {
        string? messages = null;

        if (items.Length <= 0)
            return null;

        EState oldState = GetState(items[0]);
        if (items.Any(x => x.ID_State != items[0].ID_State))
            return "Lista musi być z tym samym stanem";

        if (!items.Any(x => CanMove(x,targetStateType)))
            return "Nie można przenieść";

        ItemState targetState = _stateRepo.GetByState(targetStateType);

        if (targetState == null)
            return "Stan nie istnieje w bazie";

        foreach(StorageItem item in items)
        {
            messages = CheckMovingItem(item, targetStateType);
            if (messages != null)
                break;
            item.ID_State = targetState.ID;
        }
        if(messages != null)
            return messages;

        if(targetStateType >= EState.Reserved)
        {
            var orderItems = items.Select(x => x.OrderItem).ToList();
            if (orderItems == null || orderItems.Contains(null))
                return "Elemeny nie maja przypisanego produktu z zamówienia";

            if(orderItems.Any(x => x.ID_Order != orderItems[0].ID_Order))
                return "Produkty są z różmych zamówień";
        }
        else
        {
            foreach (StorageItem item in items)
            {
                item.OrderItem = null;
                item.ID_OrderItem = null;
            }
        }

        List<Guid> removedPackage = new List<Guid>();
        if (targetStateType >= EState.Prepared)
        {
            if (items.Any(x => GetState(x) < EState.Reserved))
                return $"Nie wszystkie produkty są zarezerwowane";

            foreach (StorageItem item in items)
            {
                if(item.ID_Package != null)
                {
                    if (!removedPackage.Contains(item.ID_Package.Value))
                        removedPackage.Add(item.ID_Package.Value);
                    item.ID_Package = null;
                }
            }
        }

        if(oldState == EState.Prepared && targetStateType == EState.Reserved)
        {
            foreach (StorageItem item in items)
            {
                if (item.Package == null)
                    return $"Przy przenoszeniu elementu ze stanu {oldState} do {targetStateType}, powinna byc przypisana paczka z produktami"; 
            }
        }

        foreach (StorageItem item in items)
        {
            _storageItemRepo.Update(item);
            item.State = targetState;
        }

        _storageItemRepo.Save();/// Zapis po to aby móc później odczytać elementy w paczce

        foreach (Guid packageId in removedPackage)
        {
            StorageItemPackage? package = _pagkageRepo.GetById(x => x.Include(i => i.Items), packageId);

            if (package != null && package.Items.Count == 0)
                _pagkageRepo.Delete(packageId);
        }

        return messages;
    }

    public List<StorageItem>? GetItemsByPackage(Guid id)
    {
        return _storageItemRepo.GetAll(x => x.Include(i => i.Product)).Where(x => x.ID_Package == id).ToList();
    }

    public bool Save()
    {
        try
        {
            _storageItemRepo.Save();
            return true;
        }catch(Exception ex)
        {
            return false;
        }
    }

    public bool CanMove(StorageItem item, EState targetStare)
    {
        EState CurState = GetState(item);
        if (!CanMove(CurState, targetStare))
            return false;

        return true;
    }

    public void Reload()
    {
        _stateRepo.ReloadContext();
        _storageItemRepo.ReloadContext();
    }

    #endregion
}

//internal class WareHouseItemService : BaseServiceWithRepository<IWareHouseItemRepository, WareHouseItem>, IWareHouseItemService
//{
//    #region Private fields

//    private readonly IItemStateRepository _stateRepo;

//    private readonly IStorageItemRepository _storageItemRepository;

//    #endregion

//    #region Constructors

//    /// <summary>
//    /// Default constructor
//    /// </summary>
//    public WareHouseItemService(IWareHouseItemRepository repository, IItemStateRepository stateRepo, IStorageItemRepository storageItemRepository):base(repository)
//    {
//        _stateRepo = stateRepo;
//        _storageItemRepository = storageItemRepository;
//    }

//    #endregion

//    #region Public Method

//    public async Task<int> GetCoutOfProduct(Product product)
//    {
//        return (await _repozitory.GetItemAsync(product.ID)).Select(x => x.Count).Sum();
//    }

//    public async Task<int> GetCoutOfProduct(Product product, ItemState state)
//    {
//        return (await _repozitory.GetItemAsync(product.ID, state.ID))?.Count ?? 0;
//    }

//    public async Task<List<Product?>> GetProductsByState(ItemState state)
//    {
//        return await _repozitory.GetProdictsWidthStateAsync(state.ID);
//    }

//    public List<WareHouseItem> GetProductsByState(Guid stateId)
//    {
//        return _repozitory.GetAll(x => x.Include(i => i.Product)).Where(x => x.ID_State == stateId).OrderBy(x => x.State).ToList();
//    }

//    public WareHouseItem? GetItem(Guid productId, Guid stateId)
//    {
//        return _repozitory.GetAll(x => x.Include(i => i.Product).Include(i => i.State)).Where(x => x.ID_State == stateId && x.ID_Product == productId).OrderBy(x => x.State).FirstOrDefault();
//    }

//    public WareHouseItem? GetItem(Guid productId, EState state)
//    {
//        IEnumerable<ItemState> states = _stateRepo.GetAll().Where(s => s.State == state);

//        return _repozitory.GetAll(x => x.Include(i => i.Product).Include(i => i.State)).Where(x => states.Any(s => s.ID == x.ID_State) && x.ID_Product == productId).OrderBy(x => x.State).FirstOrDefault();
//    }

//    public async Task<bool> ExistProduct(Guid product) => (await _repozitory.GetAllAsync()).Any(x => x.ID_Product == product);

//    public async Task<WareHouseItem> SetCountProduct(Product product, ItemState status, int count, CancellationToken token = default(CancellationToken))
//    {
//        bool flag = false;

//        if (product == null || status == null)
//            return null;

//        WareHouseItem? found = await _repozitory.GetItemAsync(product.ID, status.ID, token);

//        if (count <= 0 && found != null)
//        {
//            _repozitory.Delete(found.ID);
//            await _repozitory.SaveAsync(token);
//            return null;
//        }

//        if (found == null)
//        {
//            found = WareHouseItem.Get();
//            found.State = status;
//            found.Product = product;
//            found.ID_Product = product.ID;
//            found.ID_State = status.ID;
//            found.Count = count <= 0 ? 1 : count;
//            await _repozitory.AddAsync(found, token);
//            await _repozitory.SaveAsync(token);
//            flag = true;
//            return found;
//        }
//        found.Count = count;
//        _repozitory.Update(found);
//        await _repozitory.SaveAsync(token);
//        flag = true;
//        return found;
//    }

//    public async Task<List<WareHouseItem>> GetAllByProductAsync(Guid productid, CancellationToken token = default(CancellationToken))
//    {
//        return await _repozitory.GetItemAsync(productid, token);
//    }

//    public string MoveProductToState(ref WareHouseItem item, Guid targetState, int count)
//    {
//        string messages = null;
//        WareHouseItem targetitem = GetItem(item.ID_Product, targetState);
//        count = item.Count > count ? count : item.Count;
//        ItemState targetstate = _stateRepo.GetById(targetState);
//        bool added = false;
//        if (targetitem == null)
//        {
//            targetitem = WareHouseItem.Get();
//            targetitem.Count = 0;
//            targetitem.ID_State = targetState;
//            targetitem.ID_Product = item.ID_Product;
//            added = true;
//        }
//        if (CanMove(item.State.State, targetstate.State))
//        {
//            targetitem.Count += count;

//            if (added)
//                _repozitory.Insert(targetitem);
//            else
//                _repozitory.Update(targetitem);

//            if(item.Count == count)
//                _repozitory.Delete(item.ID);
//            else
//            {
//                item.Count -= count;
//                _repozitory.Update(item);
//            }

//            item = targetitem;
//        }
//        else
//            messages = "Nie można przenieść";
//        return messages;

//    }

//    public string MoveProductToState(ref WareHouseItem item, EState targetState, int count)
//    {
//        ItemState state = _stateRepo.GetByState(targetState);
//        if (state == null)
//            return "Stan nie istnieje";
//        else
//            return MoveProductToState(ref item, state.ID, count);
//    }

//    public bool CanMove(EState oldState, EState newState)
//    {
//        int oldStateValue = (int)oldState;
//        int newStateValue = (int)newState;

//        if ((oldStateValue & (oldStateValue - 1)) == 0 && oldStateValue < newStateValue)
//        {
//            // newState jest bezpośrednio poziom wyżej od oldState
//            return true;
//        }
//        else if ((newStateValue & (newStateValue - 1)) == 0 && newStateValue < oldStateValue)
//        {
//            // newState jest bezpośrednio poziom niżej od oldState
//            return true;
//        }
//        else
//        {
//            return false; // newState i oldState są na tym samym poziomie lub mają inne stany pośrednie
//        }
//    }

//    public List<WareHouseItem> GetProductsAvailableToRack()
//    {
//        return _repozitory.GetAll(x => x.Include(i => i.State).Include(i => i.Product)).Where(x => x.State.State == EState.InStock).ToList();
//    }

//    public bool MoveItemsToRack(IEnumerable<StorageItem> items)
//    {
//        var possible = GetProductsAvailableToRack();
//        foreach(var item in items.GroupBy(x => x.ID_Item))
//        {
//            var count = possible.Where(x=> x.ID == item.Key).Select(x => x.Count).Sum();
//            if (!(item.Count() <= count))
//                return false;

//        }

//        Guid IdState = _stateRepo.GetAll().FirstOrDefault(x => x.State == EState.Available).ID;

//        foreach (var item in items.GroupBy(x => x.ID_Item))
//        {
//            var whitem = GetById(item.Key);
//            MoveProductToState(ref whitem, IdState, item.Count());
//            foreach (var itemState in item)
//            {
//                itemState.ID_Item = whitem.ID;
//                _storageItemRepository.Add(itemState);
//            }
//            _storageItemRepository.Save();


//        }

//        return true;
//    }

//    public bool RemoveFromRack(StorageItem item)
//    {
//        if (item.ID_OrderItem != null)
//            return false;

//        Guid IdState = _stateRepo.GetAll().FirstOrDefault(x => x.State == EState.InStock).ID;
//        var whitem =  GetById(item.ID_Item);
//        if(MoveProductToState(ref whitem, IdState, 1) == null)
//        {
//            _storageItemRepository.Delete(item.ID);
//            _storageItemRepository.Save();
//            return true;
//        }
//        return false;
//    }

//    #endregion
//}
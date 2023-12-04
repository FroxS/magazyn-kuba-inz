using Warehouse.Repository.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using Warehouse.Models.Enums;

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
            return true;
        else
        {
            if (newState > oldState)
                return true;

            return true; /*false*/ /// Pominęto sprawdzanie czy jest to poprzeni san. Zostawiono to tylko dla podnoszenia stanów; 
        }

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

    public bool AddFromSupplierOrder(Order order)
    {
        if (order == null)
            return false;

        if (order.Type != EOrderType.Supplier)
            return false;

        ItemState stateobj = _stateRepo.GetByState(EState.Delivery);
        if (stateobj == null)
            return false;

        List<OrderProduct> prodictItems = new List<OrderProduct>();
        if (order.Items == null)
            prodictItems = _orderProductRepo.GetAll().Where(x => x.ID_Order == order.ID).ToList();
        else
            prodictItems = order.Items;

        foreach(OrderProduct prodictItem in prodictItems)
        {
            StorageItem item = StorageItem.Get();
            item.ID_State = stateobj.ID;
            item.ID_Product = prodictItem.ID_Product;
            item.ID_OrderItem = prodictItem.ID;

            prodictItem.ID_StorageItem = item.ID;

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
        if (targetStateType >= EState.Available && targetStateType < EState.TransportReady)
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
                return $"Produkt nie ma przypsanego prduktu z oferty {targetState.Name}";
        }
        else
        {
            if(item.State.State > targetStateType)
            {
                item.ID_OrderItem = null;
                item.OrderItem = null;
            }
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

        List<Guid> removedPackage = new List<Guid>();

        if(targetStateType == EState.InStock)
        {
            foreach (StorageItem item in items)
            {
                item.OrderItem = null;
                item.ID_OrderItem = null;
            }
        }

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
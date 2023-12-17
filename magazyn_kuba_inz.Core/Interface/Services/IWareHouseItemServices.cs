using System.Collections.ObjectModel;
using Warehouse.Core.Helpers;
using Warehouse.Core.Models;
using Warehouse.Core.Models.Settings;
using Warehouse.Core.Resources;
using Warehouse.Models;
using Warehouse.Models.Enums;

namespace Warehouse.Core.Interface;

public interface IUserService : IBaseService<User>
{
    User GetUser(RegisterResource resource);
    Task<User> Register(RegisterResource resource, CancellationToken cancellationToken = default(CancellationToken));
    Task<User> Login(LoginResource resource, CancellationToken cancellationToken = default(CancellationToken));
    Task<List<User>> GetUsers(CancellationToken cancellationToken = default);
    bool ChangePassword(ChangePassworldResource resource);
}

public interface IProductService : IBaseService<Product> 
{
    Task<Product> AddAsync(Product item);
    Task<Product> SetImage(Product product, byte[] imgBytes);
    bool ExistOnWareHouse(Guid id);
    double GetPrice(Guid id);
}

public interface IProductStatusService : IBaseService<ProductStatus> 
{
    Task<ProductStatus> AddAsync(ProductStatus item);
}

public interface IProductGroupService : IBaseService<ProductGroup>
{
    Task<ProductGroup> AddAsync(ProductGroup item);
    Task<ObservableCollection<ProductGroup>> GetAllProductGroupToViewModel();
}

public interface ISupplierService : IBaseService<Supplier>
{
}

public interface IItemStateService : IBaseService<ItemState>
{
    Task<ItemState> AddAsync(ItemState supplier);
    ItemState GetByState(EState state);
}

public interface IWareHouseService
{
    bool Save();
    bool CanAddProduct(Guid product, EState state);
    bool Add(Guid product, EState state, int count);
    string? MoveProductToState(EState targetStateType, params StorageItem[] items);
    List<StorageItem> GetProductsByState(EState state);
    bool ExistProduct(Guid idProduct);
    bool CanMove(StorageItem item, EState targetStare);
    List<StorageItem>? GetItemsByPackage(Guid id);
    void Reload();
    bool AddFromSupplierOrder(Order order);
    List<StorageItem> GetProductsByStateWithOrders(EState state);
}

public interface IImageService : IBaseService<WareHouseImage> { }

public interface IOrderService : IBaseService<Order> 
{
    List<OrderProduct> GetProducts(Guid id);
    List<Order> GetAll(EOrderType type = EOrderType.WareHouse | EOrderType.Supplier);
    Task<List<Order>> GetAllAsync(EOrderType type = EOrderType.WareHouse | EOrderType.Supplier, CancellationToken cancellationToken = default);
    void SetWay(List<WayObject> way, Order order);
    List<WayObject>? GetWay(Order order);
    bool IsReserved(Guid id);
    bool IsPrepared(Guid id);
    bool IsReceived(Guid id);
    bool SetAsPrepared(Order order, IWareHouseService service);
    bool Reserv(Order order, IWareHouseService service, ref string message);
    bool SetAsReceived(Order order, IWareHouseService service);
    string GetNewOrderName(EOrderType type = EOrderType.WareHouse);
    double TotalPrice(Order order);
    double TotalPrice(Guid id);
    EOrderState GetState(Guid id);
}

public interface IOrderProductService : IBaseService<OrderProduct> { }

public interface IStorageUnitService : IBaseService<StorageUnit> { }

public interface IRackService : IBaseService<Rack> 
{
    bool CanDeleteRack(Guid id);
    List<Rack> GetAllWithItems();
    List<StorageItemPackage> GetAllPackages(Guid id);
    Rack GetWithItems(Guid id);
    IEnumerable<StorageItem>? GetItems(Guid id);
}

public interface IStorageItemPackageService : IBaseService<StorageItemPackage> 
{
    List<StorageItem>? GetItemsByID(Guid id);
}
public interface IStorageItemService : IBaseService<StorageItem> 
{
    List<StorageItem>? GetItemsByPackage(Guid id);
}

public interface IHallService : IBaseService<Hall> 
{
    HallObject GetHallObject(Guid id);
    string? IsHallOk(HallObject obj);
    bool UpdatehHllObject(HallObject obj);

    Hall GetHall(HallObject obj);
}

public interface IAppSettingsService
{
    GlobalSettings GetSettings();

    void SaveSettings(GlobalSettings settings);
}


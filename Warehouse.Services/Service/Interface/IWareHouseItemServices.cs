using System.Collections.ObjectModel;
using Warehouse.Core.Interface;
using Warehouse.Core.Models;
using Warehouse.Core.Resources;
using Warehouse.Models;
using Warehouse.Models.Interfaces;

namespace Warehouse.Service.Interface;

public interface IUserService
{
    Task<UserResource> Register(RegisterResource resource, CancellationToken cancellationToken = default(CancellationToken));
    Task<IUser> Login(LoginResource resource, CancellationToken cancellationToken = default(CancellationToken));
}

public interface IProductService : IBaseService<Product> 
{
    Task<Product> AddAsync(Product item);
    Task<Product> SetImage(Product product, byte[] imgBytes);
    Task<bool> ExistOnWareHouse(Guid id);
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
    Supplier Add(Supplier supplier);
}

public interface IItemStateService : IBaseService<ItemState>
{
    Task<ItemState> AddAsync(ItemState supplier);
}

public interface IWareHouseItemService : IBaseService<WareHouseItem>
{
    Task<List<WareHouseItem>> GetAllByProductAsync(Guid productid, CancellationToken token = default(CancellationToken));
    Task<WareHouseItem> SetCountProduct(Product product, ItemState status, int count, CancellationToken token = default(CancellationToken));
    Task<List<Product?>> GetProductsByState(ItemState state);
    Task<int> GetCoutOfProduct(Product product, ItemState state);
    Task<int> GetCoutOfProduct(Product product);
    Task<bool> ExistProduct(Guid product);
    List<WareHouseItem> GetProductsByState(Guid stateId);
    WareHouseItem? GetItem(Guid productId, Guid stateId);
    string MoveProductToState(ref WareHouseItem item, Guid targetState, int count);
    List<WareHouseItem> GetProductsAvailableToRack();
    bool MoveItemsToRack(IEnumerable<StorageItem> items);
    bool RemoveFromRack(StorageItem item);
}

public interface IImageService : IBaseService<WareHouseImage> { }
public interface IOrderService : IBaseService<Order> { }
public interface IOrderProductService : IBaseService<OrderProduct> { }
public interface IStorageUnitService : IBaseService<StorageUnit> { }
public interface IRackService : IBaseService<Rack> 
{
    bool CanDeleteRack(Guid id);
    List<Rack> GetAllWithItems();
}
public interface IStorageItemPackageService : IBaseService<StorageItemPackage> 
{
    List<StorageItem>? GetItemsByID(Guid id);
}
public interface IStorageItemService : IBaseService<StorageItem> { }

public interface IHallService : IBaseService<Hall> 
{
    HallObject GetHallObject(Guid id);
    string? IsHallOk(HallObject obj);
    bool UpdatehHllObject(HallObject obj);

    Hall GetHall(HallObject obj);
}


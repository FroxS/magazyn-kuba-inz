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
    //Task<WareHouseItemViewModel> GetWareHouseItemVM(WareHouseItem item);
    //Task<WareHouseItemViewModel> CreateWareHouseItemVM();
    //Task<List<WareHouseItemViewModel>> GetAllWareHouseItemToViewModel();
    Task<List<WareHouseItem>> GetAllByProductAsync(Guid productid, CancellationToken token = default(CancellationToken));
    Task<WareHouseItem> RemoveProduct(Product product, ItemState status,  CancellationToken token = default(CancellationToken));
    Task<WareHouseItem> AddProduct(Product product, ItemState status, CancellationToken token = default(CancellationToken));
    Task<WareHouseItem> SetCountProduct(Product product, ItemState status, int count, CancellationToken token = default(CancellationToken));
    Task<List<Product?>> GetProductsByState(ItemState state);
    Task<int> GetCoutOfProduct(Product product, ItemState state);
    Task<int> GetCoutOfProduct(Product product);
    Task<bool> ExistProduct(Guid product);
}

public interface IImageService : IBaseService<WareHouseImage> { }
public interface IOrderService : IBaseService<Order> { }
public interface IOrderProductService : IBaseService<OrderProduct> { }
public interface IStorageUnitService : IBaseService<StorageUnit> { }
public interface IRackService : IBaseService<Rack> 
{
    bool CanDeleteRack(Guid id);
}
public interface IStorageItemPackageService : IBaseService<StorageItemPackage> { }
public interface IStorageItemService : IBaseService<StorageItem> { }

public interface IHallService : IBaseService<Hall> 
{
    HallObject GetHallObject(Guid id);
    string? IsHallOk(HallObject obj);
    bool UpdatehHllObject(HallObject obj);

    Hall GetHall(HallObject obj);
}


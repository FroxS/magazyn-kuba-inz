using magazyn_kuba_inz.Core.Models;
using magazyn_kuba_inz.Core.Resources;
using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.Models.Interfaces;
using magazyn_kuba_inz.Models.WareHouse;
using System.Collections.ObjectModel;

namespace magazyn_kuba_inz.Core.Service.Interface;

public interface IBaseService<Model> where Model : class
{
    /// <summary>
    /// Delete Entity by Id
    /// </summary>
    /// <param name="id">Id of this entity</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

    bool Delete(Guid id);

    /// <summary>
    /// Method to gel all entities
    /// </summary>
    /// <returns></returns>
    Task<List<Model>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

    List<Model> GetAll();

    /// <summary>
    /// Method to get Entity by Id
    /// </summary>
    /// <param name="id">Id of entity</param>
    /// <returns></returns>
    Task<Model> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Method to add entity to database
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AddAsync(Model entity, CancellationToken cancellationToken = default(CancellationToken));

    bool Add(Model entity);

    /// <summary>
    /// Method to update database
    /// </summary>
    /// <param name="entity">Entity to update</param>
    void Update(Model entity);

    Task<bool> SaveAsync(CancellationToken cancellationToken = default(CancellationToken));

    bool Save();

    void RefreshDbContext();

    bool Exist(Guid id);
}

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
    Task<WareHouseItemViewModel> GetWareHouseItemVM(WareHouseItem item);
    Task<WareHouseItemViewModel> CreateWareHouseItemVM();
    Task<List<WareHouseItemViewModel>> GetAllWareHouseItemToViewModel();
    Task<List<WareHouseItemViewModel>> GetAllWareHouseItemToViewModel(Guid productid);
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


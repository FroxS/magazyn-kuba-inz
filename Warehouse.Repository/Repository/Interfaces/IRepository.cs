﻿using Warehouse.Models;
using Warehouse.Models.Enums;

namespace Warehouse.Repository.Interfaces;

public interface IItemStateRepository : IBaseRepository<ItemState> 
{
    ItemState GetByState(EState state);
}
public interface IProductStatusRepository : IBaseRepository<ProductStatus> { }
public interface IProductGroupRepository : IBaseRepository<ProductGroup> 
{
    Task<ProductGroup?> GetProductGroupWithProducts(Guid id, CancellationToken cancellationToken = default(CancellationToken));
}
public interface ISupplierRepository : IBaseRepository<Supplier> { }
//public interface IWareHouseItemRepository : IBaseRepository<WareHouseItem> 
//{
//    Task<WareHouseItem?> GetItemAsync(Guid productId, Guid stateId, CancellationToken cancellationToken = default);
//    Task<List<WareHouseItem>> GetItemAsync(Guid productId, CancellationToken cancellationToken = default);
//    Task<List<Product?>> GetProdictsWidthStateAsync(Guid statusId, CancellationToken cancellationToken = default);
//    WareHouseItem? GetItem(Guid productId, Guid stateId);
//}
public interface IProductRepository : IBaseRepository<Product> 
{
}
public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken = default(CancellationToken));
    User? GetByLogin(string login);
}
public interface IImageRepository : IBaseRepository<WareHouseImage>
{
}
public interface IOrderRepository : IBaseRepository<Order> { }
public interface IOrderProductRepository : IBaseRepository<OrderProduct> { }
public interface IStorageUnitRepository : IBaseRepository<StorageUnit> { }
public interface IStorageItemPackageRepository : IBaseRepository<StorageItemPackage> { }
public interface IRackRepository : IBaseRepository<Rack> { }
public interface IStorageItemRepository : IBaseRepository<StorageItem> { }
public interface IHallRepository : IBaseRepository<Hall> { }
public interface IAppSettingsRepository : IBaseRepository<AppSettings> { }
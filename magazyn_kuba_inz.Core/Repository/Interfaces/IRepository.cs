using magazyn_kuba_inz.Models.WareHouse;

namespace magazyn_kuba_inz.Core.Repository.Interfaces;

public interface IItemStateRepository : IBaseRepository<ItemState> { }
public interface IProductStatusRepository : IBaseRepository<ProductStatus> { }
public interface IProductGroupRepository : IBaseRepository<ProductGroup> { }
public interface ISupplierRepository : IBaseRepository<Supplier> { }
public interface IWareHouseItemRepository : IBaseRepository<WareHouseItem> { }
public interface IProductRepository : IBaseRepository<Product> { }
public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetByNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken));
}
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Repository.Interfaces;

namespace Warehouse.Repository
{
    public static class RepositoryService
    {
        #region Static Methods

        public static IServiceCollection PrepareRepository(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IItemStateRepository, ItemStateRepository>();
            services.AddTransient<IProductGroupRepository, ProductGroupRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductStatusRepository, ProductStatusRepository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
            //services.AddTransient<IWareHouseItemRepository, WareHouseItemRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderProductRepository, OrderProductRepository>();
            services.AddTransient<IRackRepository, RackRepository>();
            services.AddTransient<IStorageUnitRepository, StorageUnitRepository>();
            services.AddTransient<IStorageItemPackageRepository, StorageItemPackageRepository>();
            services.AddTransient<IStorageItemRepository, StorageItemRepository>();
            services.AddTransient<IHallRepository, HallRepository>();
            return services;
        }

        #endregion
    }
}
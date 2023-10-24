using Microsoft.Extensions.DependencyInjection;
using Warehouse.Service.Interface;

namespace Warehouse.Service
{
    public static class Service
    {
        #region Static Methods

        public static IServiceCollection PrepareService(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IProductGroupService, ProductGroupService>();
            services.AddTransient<IProductStatusService, ProductStatusService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IItemStateService, ItemStateService>();
            services.AddTransient<IWareHouseItemService, WareHouseItemService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderProductService, OrderProductService>();
            services.AddTransient<IRackService, RackService>();
            services.AddTransient<IStorageUnitService, StorageUnitService>();
            services.AddTransient<IStorageItemPackageService, StorageItemPackageService>();
            services.AddTransient<IStorageItemService, StorageItemService>();
            services.AddTransient<IHallService, HallService>();

            services.AddSingleton<MessageService>();

            return services;
        }

        #endregion
    }
}
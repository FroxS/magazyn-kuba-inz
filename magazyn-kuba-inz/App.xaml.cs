using magazyn_kuba_inz.Core.Repository;
using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.Core.Service;
using magazyn_kuba_inz.Core.Service.Dialog;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel;
using magazyn_kuba_inz.Core.ViewModel.Dialog;
using magazyn_kuba_inz.Core.ViewModel.Login;
using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.EF;
using magazyn_kuba_inz.View;
using magazyn_kuba_inz.View.Dialog;
using magazyn_kuba_inz.View.Login;
using magazyn_kuba_inz.View.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;

namespace magazyn_kuba_inz;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public IConfiguration Configuration { get; private set; }
    public App()
    {
        AppHost = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
        {
            PrepareDatabase(services);
            PrepareService(services);
            PreparePages(services);
            PrepareRepository(services);
            PrepareViewModels(services);
            PrepareViews(services);
            PrepareApplication(services);

        }).Build();
    }

    protected async override void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();
        Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        IApp app = AppHost.Services.GetRequiredService<IApp>();
        app.Run();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }

    private void PrepareService(IServiceCollection services)
    {
        services.AddTransient<IUserService,UserService>();
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
        services.AddSingleton<IDialogService, DialogService>();
        services.AddTransient((o) => { return Dispatcher; });
    }

    private void PrepareApplication(IServiceCollection services)
    {
        services.AddSingleton<Application>((o) => { return this; });
        services.AddSingleton<INavigation, NavigationViewModel>((o) => {
            return new NavigationViewModel() { AppHost = AppHost };
        });
        services.AddSingleton<IApp, ApplicationViewModel>();
        services.AddSingleton<IInnerDialogService,InnerDialogService>();
    }

    private void PrepareViews(IServiceCollection services)
    {
        services.AddTransient<IMainWindow, MainWindow>();
        services.AddSingleton<ILoginWindow, LoginView>();
        services.AddSingleton<IRegisterWindow, RegisterView>();

        services.AddTransient<IDialogWindow, BaseDialogWindow>();

        services.AddTransient<IYesNoDialogView,YesNoDialogView>();
        services.AddTransient<IAlertDialogView,AlertDialogView>();
    }

    private void PrepareViewModels(IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<RegisterViewModel>();
        services.AddTransient<DashBoardPageViewModel>();
        services.AddTransient<ProductsPageViewModel>();
        services.AddTransient<ProductGroupsPageViewModel>();
        services.AddTransient<ProductStatusesPageViewModel>();
        services.AddTransient<SuppliersPageViewModel>();
        services.AddTransient<ItemStatesPageViewModel>();
        services.AddTransient<SettingsPageViewModel>();
        services.AddTransient<WareHouseItemsPageViewModel>();
        services.AddTransient<StorageUnitsPageViewModel>();
        services.AddTransient<WareHouseCreatorPageViewModel>();
    }

    private void PrepareRepository(IServiceCollection services)
    {
        services.AddTransient<IUserRepository,UserRepository>();
        services.AddTransient<IItemStateRepository, ItemStateRepository> ();
        services.AddTransient<IProductGroupRepository, ProductGroupRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IProductStatusRepository, ProductStatusRepository>();
        services.AddTransient<ISupplierRepository, SupplierRepository>();
        services.AddTransient<IImageRepository, ImageRepository>();
        services.AddTransient<IWareHouseItemRepository, WareHouseItemRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IOrderProductRepository, OrderProductRepository>();
        services.AddTransient<IRackRepository, RackRepository>();
        services.AddTransient<IStorageUnitRepository, StorageUnitRepository>();
        services.AddTransient<IStorageItemPackageRepository, StorageItemPackageRepository>();
        services.AddTransient<IStorageItemRepository, StorageItemRepository>();
        services.AddTransient<IHallRepository, HallRepository>();
    }

    private void PrepareDatabase(IServiceCollection services)
    {
        services.AddDbContextFactory<WarehouseDbContext>();
    }

    private void PreparePages(IServiceCollection services)
    {
        services.AddTransient<DashBoardPage>();
        services.AddTransient<ProductsPage>();
        services.AddTransient<ProductGroupPage>();
        services.AddTransient<SuppliersPage>();
        services.AddTransient<SettingsPage>();
        services.AddTransient<ProductStatusesPage>();
        services.AddTransient<ItemStatesPage>();
        services.AddTransient<WareHouseItemsPage>();
        services.AddTransient<StorageUnitsPage>();
        services.AddTransient<WareHouseCreatorPage>();
    }

    private void AddCOnfiguration(IServiceCollection services)
    {
        var builder = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Configuration = builder.Build();
        services.AddSingleton(Configuration);
    }
}


using Warehouse.Repository;
using Warehouse.EF;
using Warehouse.View;
using Warehouse.View.Dialog;
using Warehouse.View.Login;
using Warehouse.View.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;
using Warehouse.Service.Interface;
using Warehouse.Service;
using Warehouse.Core.Interface;
using Warehouse.Dialog;
using Warehouse.ViewModel;
using Warehouse.ViewModel.Pages;
using Warehouse.ViewModel.Login;

namespace Warehouse;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
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
            services.PrepareRepository();
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
        services.AddSingleton<System.Windows.Application>((o) => { return this; });
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

        services.AddTransient<IYesNoDialogView, YesNoDialogView>();
        services.AddTransient<IAlertDialogView, AlertDialogView>();
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
        services.AddTransient<RacksPageViewModel>();
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
        services.AddTransient<RacksPage>();
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


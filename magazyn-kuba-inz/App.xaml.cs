using magazyn_kuba_inz.Core.Repository;
using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.Core.Service;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel;
using magazyn_kuba_inz.Core.ViewModel.Login;
using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.EF;
using magazyn_kuba_inz.View;
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
        services.AddScoped<IUserService,UserService>();
    }

    private void PrepareApplication(IServiceCollection services)
    {
        services.AddSingleton<Application>((o) => { return this; });
        services.AddSingleton<INavigation, NavigationViewModel>((o) => {
            return new NavigationViewModel() { AppHost = AppHost };
        });
        services.AddSingleton<IApp, ApplicationViewModel>();
    }

    private void PrepareViews(IServiceCollection services)
    {
        services.AddTransient<IMainWindow, MainWindow>();
        services.AddSingleton<ILoginWindow, LoginView>();
        services.AddSingleton<IRegisterWindow, RegisterView>();
    }

    private void PrepareViewModels(IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<RegisterViewModel>();

        services.AddSingleton<DashBoardPageViewModel>();
        services.AddSingleton<ProductsPageViewModel>();
        services.AddSingleton<ProductGroupPageViewModel>();
        services.AddSingleton<SuppliersPageViewModel>();
    }

    private void PrepareRepository(IServiceCollection services)
    {
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IItemStateRepository, ItemStateRepository> ();
        services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductStatusRepository, ProductStatusRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IWareHouseItemRepository, WareHouseItemRepository>();
    }

    private void PrepareDatabase(IServiceCollection services)
    {
        services.AddDbContext<WarehouseDbContext>();
    }

    private void PreparePages(IServiceCollection services)
    {
        services.AddTransient<DashBoardPage>();
        services.AddTransient<ProductsPage>();
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


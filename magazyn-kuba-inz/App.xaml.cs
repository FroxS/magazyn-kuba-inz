using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel;
using magazyn_kuba_inz.Core.ViewModel.Login;
using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.EF;
using magazyn_kuba_inz.View;
using magazyn_kuba_inz.View.Login;
using magazyn_kuba_inz.View.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace magazyn_kuba_inz;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
        {
            PrepareDatabase(services);
            PrepareService(services);
            PreparePages(services);
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
        services.AddSingleton<Application>((o)=> { return this; });
        services.AddSingleton<INavigation, NavigationViewModel>((o) => {
            return new NavigationViewModel() { AppHost = AppHost };
        });
        services.AddSingleton<IApp, ApplicationViewModel>();
        services.AddSingleton<IMainWindow,MainWindow>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<DashBoardViewModel>();
        services.AddSingleton<ILoginWindow,LoginView>();
        services.AddSingleton<LoginViewModel>();

    }

    private void PrepareDatabase(IServiceCollection services)
    {
        services.AddDbContext<WarehouseDbContext>();

    }

    private void PreparePages(IServiceCollection services)
    {
        services.AddTransient<DashBoardPage>();
    }


}


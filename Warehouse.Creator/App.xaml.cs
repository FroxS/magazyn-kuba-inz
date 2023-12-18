﻿using Warehouse.Repository;
using Warehouse.EF;
using Warehouse.View;
using Warehouse.View.Login;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Warehouse.Service;
using Warehouse.Core.Interface;
using Warehouse.Dialog;
using Warehouse.ViewModel;
using Warehouse.ViewModel.Pages;
using Warehouse.ViewModel.Login;
using Warehouse.Core.Models.Settings;

namespace Warehouse.Creator
{
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
				services.PrepareRepository();
				services.PrepareService();
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
			var sp = AppHost.Services.GetRequiredService<ISplashScreen>();
			(app as WareHouseApp)._splashScreen = sp;
			sp.Show();
			await app.Run();
			base.OnStartup(e);
		}

		protected override async void OnExit(ExitEventArgs e)
		{
			await AppHost!.StopAsync();
			base.OnExit(e);
		}

		private void PrepareService(IServiceCollection services)
		{

			services.AddSingleton<IDialogService, DialogService>();
			services.AddTransient((o) => { return Dispatcher; });

			/// Settings
			services.AddSingleton<UserSettings>();
			services.AddSingleton<GlobalSettings>();
		}

		private void PrepareApplication(IServiceCollection services)
		{
			services.AddSingleton<System.Windows.Application>((o) => { return this; });
			services.AddSingleton<IApp, WarehouseCreator>();
		}

		private void PrepareViews(IServiceCollection services)
		{
			services.AddTransient<IMainWindow, CreatorWindow>();
			services.AddTransient<IRegisterWindow, RegisterView>();
			services.AddTransient<ISplashScreen, View.Login.SplashScreen>();
		}

		private void PrepareViewModels(IServiceCollection services)
		{
			//services.AddSingleton<MainViewModel>();
			//services.AddSingleton<LoginViewModel>();
			//services.AddSingleton<RegisterViewModel>();
			//services.AddSingleton<DashBoardPageViewModel>();
			//services.AddSingleton<ProductsPageViewModel>();
			//services.AddSingleton<ProductGroupsPageViewModel>();
			//services.AddSingleton<ProductStatusesPageViewModel>();
			//services.AddSingleton<SuppliersPageViewModel>();
			//services.AddSingleton<ItemStatesPageViewModel>();
			//services.AddSingleton<SettingsPageViewModel>();
			//services.AddSingleton<WareHousePageViewModel>();
			//services.AddSingleton<StorageUnitsPageViewModel>();
			//services.AddSingleton<WareHouseCreatorPageViewModel>();
			//services.AddSingleton<RacksPageViewModel>();
			//services.AddSingleton<UserPageViewModel>();
			//services.AddSingleton<UsersPageViewModel>();
			//services.AddTransient<OrdersPageViewModel>();
			//services.AddTransient<RackEditViewModel>();
		}

		private void PrepareDatabase(IServiceCollection services)
		{
			services.AddDbContextFactory<WarehouseDbContext>();
		}
	}

}
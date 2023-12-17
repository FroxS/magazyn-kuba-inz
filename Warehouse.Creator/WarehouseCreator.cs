using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Warehouse.Core.Interface;
using Warehouse.Core.Resources;
using Warehouse.Models;
using Warehouse.Models.Interfaces;
using Warehouse.Theme;

namespace Warehouse.Creator
{
	internal class WarehouseCreator : WareHouseApp
	{
		#region Private Properties

		#endregion

		#region Public Properties

		#endregion

		#region Constructors

		public WarehouseCreator(Application app, IServiceProvider services) : base(app,services)
		{
			

		}

		#endregion

		#region Public Methods

		public override async Task Run()
		{
			SetTheme(GetUserSettings()?.ColorScheme ?? ColorScheme.Dark);

			int count = GetService<IUserService>().GetAll().Count;

			if(count == 0)
			{
				IRegisterWindow register = _services.GetRequiredService<IRegisterWindow>();
				register.ExitOnSuccesfulRegister = true;
				register.LoginOnSuccefulRegister = true;
				MainWindow = register as Window;
				CloseSplashForm();
				if (register.ShowDialog() != true)
				{
					app.Shutdown();
					return;
				}

				User reguserUesr = (User)register.GetUser();
				if (register.GetUser() == null)
				{
					app.Shutdown();
					return;
				} 
				
			}

			bool? flag = false;

			if (!IsUserLogin())
			{
				_services.GetRequiredService<ILoginWindow>();
				ILoginWindow login = _services.GetRequiredService<ILoginWindow>();
				if (login == null)
					throw new Exception(Core.Properties.Resources.ErrorLoginWindowNotExist);

				MainWindow = login as Window;
				CloseSplashForm();
				flag = login.ShowDialog();
			}

			if (flag == false)
			{
				app.Shutdown();
				return;
			}
			if (User == null)
			{
				await Run();
			}
			else
			{
				ClearSilentMessage();
				IMainWindow window = _services.GetRequiredService<IMainWindow>();
				if (window == null)
					throw new Exception(Core.Properties.Resources.ErrorMainWindowNotExist);
				MainWindow = app.MainWindow = window as Window;
				CloseSplashForm();
				window.Show();
			}
		}

		#endregion
	}
}

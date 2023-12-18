using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Warehouse.Core.Interface;
using Warehouse.Creator.ViewModel;
using Warehouse.Theme;

namespace Warehouse.Creator.Service
{
    internal class WarehouseCreator : WareHouseApp
    {
        #region Private Properties

        #endregion

        #region Public Properties

        #endregion

        #region Constructors

        public WarehouseCreator(Application app, IServiceProvider services) : base(app, services)
        {


        }

        #endregion

        #region Public Methods

        public override async Task Run()
        {
            SetTheme(GetUserSettings()?.ColorScheme ?? ColorScheme.Dark);

            ClearSilentMessage();
            IMainWindow window = _services.GetRequiredService<IMainWindow>();
            window.DataContext = new CreatorWindowViewModel(this);
            if (window == null)
                throw new Exception(Core.Properties.Resources.ErrorMainWindowNotExist);
            MainWindow = app.MainWindow = window as Window;
            CloseSplashForm();
            window.Show();
        }

        #endregion
    }
}

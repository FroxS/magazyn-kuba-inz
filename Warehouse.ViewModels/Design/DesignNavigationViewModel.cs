using System.Collections.ObjectModel;
using Warehouse.Models.Page;
using Warehouse.ViewModel.Pages;
using Warehouse.ViewModels.Navigation;

namespace Warehouse.ViewModel.Design
{
    public class DesignNavigationViewModel : NavigationViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static DesignNavigationViewModel Instance => new DesignNavigationViewModel();

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public DesignNavigationViewModel()
        {
            //SetPage(new DashBoardPageViewModel(null,null,null,null));// EApplicationPage.DashBoard;
            MenuItems = new ObservableCollection<CustomMenuItem>()
            {
                new CustomMenuItem(this,EApplicationPage.DashBoard, Warehouse.Core.Properties.Resources.Home),
                new CustomMenuItem(this,EApplicationPage.Order, Warehouse.Core.Properties.Resources.Orders),
                new CustomMenuItem(this,EApplicationPage.WareHouseItems, Warehouse.Core.Properties.Resources.WareHouseItems),
                new CustomMenuItem(this,EApplicationPage.Products, Warehouse.Core.Properties.Resources.Products)
                {
                    Items = new ObservableCollection<CustomMenuItem>()
                    {
                        new CustomMenuItem(this,EApplicationPage.Suppliers, Warehouse.Core.Properties.Resources.Suppliers),
                        new CustomMenuItem(this,EApplicationPage.ProductGroups, Warehouse.Core.Properties.Resources.Group),
                        new CustomMenuItem(this,EApplicationPage.ProductStatuses, Warehouse.Core.Properties.Resources.Status),
                        new CustomMenuItem(this,EApplicationPage.ItemStates, Warehouse.Core.Properties.Resources.ItemState),
                    }
                },
                new CustomMenuItem(this,EApplicationPage.WareHouseCreator, Warehouse.Core.Properties.Resources.WareHouse)
                {
                    Items = new ObservableCollection<CustomMenuItem>()
                    {
                        new CustomMenuItem(this,EApplicationPage.StorageUnits, Warehouse.Core.Properties.Resources.StorageUnits),
                        new CustomMenuItem(this,EApplicationPage.Racks, Warehouse.Core.Properties.Resources.Racks),
                        new CustomMenuItem(this,EApplicationPage.WareHouseCreator, Warehouse.Core.Properties.Resources.Creator),
                    }
                },
                new CustomMenuItem(this,EApplicationPage.Settings, Warehouse.Core.Properties.Resources.Settings)
            };
        }

        #endregion
    }
}
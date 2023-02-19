using magazyn_kuba_inz.Core.Models;
using magazyn_kuba_inz.Models.Page;
using System.Collections.ObjectModel;

namespace magazyn_kuba_inz.Core.ViewModel.Design
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
            NavItems = new ObservableCollection<NavItem>()
            {
                new NavItem(NavItemType.Dashboard, Properties.Resources.Dashboard),
                new NavItem(NavItemType.Settings, Properties.Resources.Settings)
            };
            Page = ApplicationPage.DashBoard;
        }

        #endregion
    }
}
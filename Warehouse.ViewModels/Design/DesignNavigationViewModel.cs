using Warehouse.Models.Page;
using Warehouse.ViewModel.Pages;

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
            Page = new DashBoardPageViewModel(null);// EApplicationPage.DashBoard;
        }

        #endregion
    }
}
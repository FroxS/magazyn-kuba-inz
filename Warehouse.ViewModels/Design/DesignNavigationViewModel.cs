using Warehouse.Models.Page;

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
            Page = EApplicationPage.DashBoard;
        }

        #endregion
    }
}
using Warehouse.Core.Interface;
using Warehouse.Creator.ViewModel;
using Warehouse.ViewModel;

namespace Warehouse.Creator.Service
{
    public class CreatorNavigation : NavigationViewModel
    {
        #region Private properties

        #endregion

        #region Public properties

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CreatorNavigation(IApp app): base(app) 
        {
            Pages = new System.Collections.ObjectModel.ObservableCollection<IBasePageViewModel>();
            Pages.Add(new Step1ViewModel(app) { IsMain = true }) ;
            Pages.Add(new Step2ViewModel(app) { IsMain = true });
            Pages.Add(new Step3ViewModel(app) { IsMain = true });
            Pages.Add(new Step4ViewModel(app) { IsMain = true });
            Pages.Add(new Step5ViewModel(app) { IsMain = true });
            Pages.Add(new Step6ViewModel(app) { IsMain = true });
            Pages.Add(new Step7ViewModel(app) { IsMain = true });
            SetPage(Pages[0]);
        }

        #endregion
    }
}
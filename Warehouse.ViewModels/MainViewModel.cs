using Warehouse.Core.Helpers;
using Warehouse.ViewModel.Service;
using Warehouse.Models.Page;
using System.Windows.Input;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel;

public class MainViewModel: BaseViewModel
{
    #region Private Properties

    private readonly INavigation nav;

    #endregion

    #region Public Properties

    #endregion

    #region Command

    public ICommand NextPageCommand { get; private set; }

    #endregion

    #region Constructors

    public MainViewModel(INavigation nav)
    {
        this.nav = nav;
        NextPageCommand = new RelayCommand((o) => {
            nav.SetPage(EApplicationPage.DashBoard); 
        });
    }

    #endregion
}


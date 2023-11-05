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

    private bool flag = false;

    #endregion

    #region Public Properties

    #endregion

    #region Command

    public ICommand NextPageCommand { get; private set; }

    public ICommand ChangeThemeCommand { get; private set; }

    #endregion

    #region Constructors

    public MainViewModel(INavigation nav, IApp app)
    {
        this.nav = nav;
        NextPageCommand = new RelayCommand(() => {
            nav.SetPage(EApplicationPage.DashBoard); 
        });

        ChangeThemeCommand = new RelayCommand(() => {
            app.SetTheme(flag);
            flag = !flag;
        });
    }

    #endregion
}


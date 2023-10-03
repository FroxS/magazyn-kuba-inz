using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.Page;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel;

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
            nav.SetPage(ApplicationPage.DashBoard); 
        });
    }

    #endregion
}


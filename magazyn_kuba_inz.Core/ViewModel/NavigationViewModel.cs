using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.Page;
using Microsoft.Extensions.Hosting;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel;

public delegate void PageChanged(ApplicationPage page);

public class NavigationViewModel : BaseViewModel, INavigation
{
    #region Public Properties

    public IHost? AppHost { get; set; }

    public ApplicationPage Page { get; protected set; }

    public BasePageViewModel PageVM { get; set; }

    public event PageChanged PageChanged = (page) => { };

    #endregion

    #region Public Commands

    public ICommand SetPageCommand { get; private set; }

    #endregion

    #region Constructors

    public NavigationViewModel()
    {
        SetPageCommand = new RelayCommand<ApplicationPage>((o) => { SetPage(o); });
        SetPage(ApplicationPage.DashBoard);
    }

    #endregion

    #region Public Methods

    public void SetPage(ApplicationPage page)
    {
        if(page != Page)
        {
            PageVM.OnPageClose();
            if (PageVM.CanChangePage)
            {
                OnPropertyChanging(nameof(Page));
                Page = page;
                OnPropertyChanged(nameof(Page));
                PageVM.OnPageOpen();
                PageChanged?.Invoke(Page);
            }
        }
    }

    public void UpdateViewModel(BasePageViewModel vm)
    {
        PageVM = vm;
    }
    

    #endregion
}

using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.Page;
using Microsoft.Extensions.Hosting;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel;

public delegate void PageChanged(EApplicationPage page);

public class NavigationViewModel : BaseViewModel, INavigation
{
    #region Public Properties

    public IHost? AppHost { get; set; }

    public EApplicationPage Page { get; protected set; }

    public BasePageViewModel PageVM { get; set; }

    public event PageChanged PageChanged = (page) => { };

    #endregion

    #region Public Commands

    public ICommand SetPageCommand { get; private set; }

    #endregion

    #region Constructors

    public NavigationViewModel()
    {
        SetPageCommand = new RelayCommand<EApplicationPage>((o) => { SetPage(o); });
        SetPage(EApplicationPage.WareHouseCreator);
    }

    #endregion

    #region Public Methods

    public void SetPage(EApplicationPage page)
    {
        if(page != Page)
        {
            PageVM?.OnPageClose();
            if (PageVM?.CanChangePage ?? true)
            {
                OnPropertyChanging(nameof(Page));
                Page = page;
                OnPropertyChanged(nameof(Page));
                PageVM?.OnPageOpen();
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

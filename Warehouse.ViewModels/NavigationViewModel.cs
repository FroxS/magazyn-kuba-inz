using Warehouse.Core.Helpers;
using Warehouse.ViewModel.Service;
using Warehouse.Models.Page;
using Microsoft.Extensions.Hosting;
using System.Windows.Input;
using Warehouse.Core.Interface;
using Warehouse.Core.Delegate;

namespace Warehouse.ViewModel;

public class NavigationViewModel : BaseViewModel, INavigation
{
    #region Public Properties

    public IHost? AppHost { get; set; }

    public EApplicationPage Page { get; protected set; }

    public IBasePageViewModel PageVM { get; set; }

    public event PageChanged PageChanged = (page) => { };

    #endregion

    #region Public Commands

    public ICommand SetPageCommand { get; private set; }

    #endregion

    #region Constructors

    public NavigationViewModel()
    {
        SetPageCommand = new RelayCommand<EApplicationPage>((o) => { SetPage(o); });
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

    public void UpdateViewModel(IBasePageViewModel vm)
    {
        PageVM = vm;
    }  

    #endregion
}

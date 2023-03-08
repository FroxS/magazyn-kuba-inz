using magazyn_kuba_inz.Core.Models;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.Page;
using Microsoft.Extensions.Hosting;
using System.Collections.ObjectModel;

namespace magazyn_kuba_inz.Core.ViewModel;

public delegate void PageChanged(ApplicationPage page);

public class NavigationViewModel : BaseViewModel, INavigation
{
    #region Public Properties

    public IHost? AppHost { get; set; }

    public ApplicationPage Page { get; protected set; }

    public event PageChanged PageChanged = (page) => { };

    public ObservableCollection<NavItem> NavItems { get; protected set; }

    #endregion

    #region Constructors

    public NavigationViewModel()
    {
        SetPage(ApplicationPage.DashBoard);
        SetNavItems();
    }

    #endregion

    #region Public Methods

    public void SetPage(ApplicationPage page)
    {
        OnPropertyChanging(nameof(Page));
        Page = page;
        PageChanged?.Invoke(Page);
        OnPropertyChanged(nameof(Page));
    }

    public void SetNavItems()
    {
        NavItems = new ObservableCollection<NavItem>();
        NavItems.Add(new NavItem(NavItemType.Dashboard, Properties.Resources.Dashboard));
        NavItems.Add(new NavItem(NavItemType.Settings, Properties.Resources.Settings));
    }

    #endregion
}

using Warehouse.Core.Helpers;
using Warehouse.ViewModel.Service;
using Warehouse.Models.Page;
using Microsoft.Extensions.Hosting;
using System.Windows.Input;
using Warehouse.Core.Interface;
using Warehouse.Core.Delegate;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Warehouse.ViewModel.Pages;
using Warehouse.Core.Exeptions;

namespace Warehouse.ViewModel;

public class NavigationViewModel : BaseViewModel, INavigation
{

    #region Private fields

    private const int MAX_REMEMBER_PAGES = 2;
    private readonly Stack<IBasePageViewModel> _prevPages = new Stack<IBasePageViewModel>();
    private readonly Stack<IBasePageViewModel> _nextPages = new Stack<IBasePageViewModel>();
    private bool _canSetPrevPage = false;
    private bool _canSetNextPage = false;

    #endregion

    #region Public Properties

    public bool CanSetNextPage
    {
        get => _canSetNextPage;
        set { SetProperty(ref _canSetNextPage, value, nameof(CanSetNextPage)); }
    }

    public bool CanSetPrevPage
    {
        get => _canSetPrevPage;
        set { SetProperty(ref _canSetPrevPage, value, nameof(CanSetPrevPage)); }
    }

    public IHost? AppHost { get; set; }

    public IBasePageViewModel Page { get; protected set; }

    public event PageChanged PageChanged = (page) => { };

    #endregion

    #region Public Commands

    public ICommand SetPageCommand { get; private set; }
    public ICommand PrevCommand { get; private set; }
    public ICommand NextCommand { get; private set; }

    #endregion

    #region Constructors

    public NavigationViewModel()
    {
        SetPageCommand = new RelayCommand<EApplicationPage>(SetPage);
        PrevCommand = new RelayCommand(() => SetPrevPage(), () => CanSetPrevPage);
        NextCommand = new RelayCommand(() => SetNextPage(), () => CanSetNextPage);
    }

    #endregion

    #region Private methods

    public IBasePageViewModel ToBasePage(EApplicationPage page)
    {
        IServiceProvider? service = AppHost?.Services;
        if (service == null)
            Debugger.Break();

        switch (page)
        {
            case EApplicationPage.DashBoard:
                return service.GetRequiredService<DashBoardPageViewModel>();
            case EApplicationPage.Products:
                return service.GetRequiredService<ProductsPageViewModel>();
            case EApplicationPage.Suppliers:
                return service.GetRequiredService<SuppliersPageViewModel>();
            case EApplicationPage.ProductGroups:
                return service.GetRequiredService<ProductGroupsPageViewModel>();
            case EApplicationPage.Settings:
                return service.GetRequiredService<SettingsPageViewModel>();
            case EApplicationPage.ProductStatuses:
                return service.GetRequiredService<ProductStatusesPageViewModel>();
            case EApplicationPage.ItemStates:
                return service.GetRequiredService<ItemStatesPageViewModel>();
            case EApplicationPage.WareHouseItems:
                return service.GetRequiredService<WareHousePageViewModel>();
            case EApplicationPage.StorageUnits:
                return service.GetRequiredService<StorageUnitsPageViewModel>();
            case EApplicationPage.WareHouseCreator:
                return service.GetRequiredService<WareHouseCreatorPageViewModel>();
            case EApplicationPage.Racks:
                return service.GetRequiredService<RacksPageViewModel>();
            case EApplicationPage.Order:
                return service.GetRequiredService<OrderPageViewModel>();
            default:
                Debugger.Break();
                return null;
        }
    }

    #endregion

    #region Public Methods

    public void SetPage(EApplicationPage page)
    {
        SetPage(ToBasePage(page));
    }

    public void SetPage(IBasePageViewModel pageVM, bool savePrevPage = true)
    {
        var prevPage = Page;
        try
        {
            if (pageVM != null && pageVM != Page)
            {
                Page?.OnPageClose();
                if (Page?.CanChangePage ?? true)
                {
                    if (Page != null && savePrevPage)
                        AddPrevPage(Page);
                    OnPropertyChanging(nameof(Page));
                    AppHost.Services.GetService<IApp>().ReloadDatabase();
                    Page = pageVM;
                    if (Page is IPageReloadViewModel reload)
                        reload.Reload();
                    OnPropertyChanged(nameof(Page));
                    Page?.OnPageOpen();
                    PageChanged?.Invoke(Page.Page);
                }
            }
            CanSetNextPage = _nextPages.Any();
            CanSetPrevPage = _prevPages.Any();
        }
        catch (PageExeption ex)
        {
            AppHost.Services.GetService<IApp>().CatchExeption(ex);
            if (ex.PageVM == null)
                SetPage(ex.Page);
            else
                SetPage(ex.PageVM);
        }
        catch (Exception ex)
        {
            AppHost.Services.GetService<IApp>().CatchExeption(ex);
            SetPage(prevPage);

        }
    }

    private void AddPrevPage(IBasePageViewModel page)
    {
        _prevPages.Push(page);
    }

    public void SetPrevPage()
    {
        if (_prevPages.Any())
        {
            var prevPage = _prevPages.Pop();
            _nextPages.Push(Page);
            SetPage(prevPage, false);
        }
    }

    public void SetNextPage()
    {
        if (_nextPages.Any())
        {
            var nextPage = _nextPages.Pop();
            _prevPages.Push(Page);
            SetPage(nextPage, false);
        }
    }


    #endregion
}

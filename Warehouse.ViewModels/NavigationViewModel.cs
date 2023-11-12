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
using System.Collections.ObjectModel;

namespace Warehouse.ViewModel;

public class NavigationViewModel : BaseViewModel, INavigation
{
    #region Private fields

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

    public IBasePageViewModel Page
    {
        get => Pages?.FirstOrDefault(x => x.IsMain);
    }

    public IBasePageViewModel ActivePage { get; set; }

    public ObservableCollection<IBasePageViewModel> Pages { get; protected set; } = new ObservableCollection<IBasePageViewModel>();

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
                return service.GetRequiredService<OrdersPageViewModel>();
            default:
                Debugger.Break();
                return null;
        }
    }

    #endregion

    #region Public Methods

    public void AddPage(IBasePageViewModel page)
    {
        var prevPage = Page;
        try
        {
            AppHost.Services.GetService<IApp>().ReloadDatabase();
            page.IsMain = false;
            Pages.Add(page);
            ActivePage = page;
            page.CloseRequest += Page_CloseRequest;
            if (Page is IPageReloadViewModel reload)
                reload.Reload();
            OnPropertyChanged(nameof(Page));
            OnPropertyChanged(nameof(Pages));
            OnPropertyChanged(nameof(ActivePage));
            page?.OnPageOpen();
            PageChanged?.Invoke(page.Page);
            CanSetNextPage = _nextPages.Any();
            CanSetPrevPage = _prevPages.Any();
        }
        catch (Exception ex)
        {
            AppHost.Services.GetService<IApp>().CatchExeption(ex);
        }
    }

    public void SetPage(EApplicationPage page)
    {
        SetPage(ToBasePage(page));
    }

    public void SetPage(IBasePageViewModel pageVM)
    {
        SetPage(pageVM,true);
    }

    private void SetPage(IBasePageViewModel pageVM, bool savePrevPage = true)
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
                    OnPropertyChanged(nameof(Pages));
                    AppHost.Services.GetService<IApp>().ReloadDatabase();
                    pageVM.IsMain = true;
                    if (Page != null)
                        Page.CloseRequest -= Page_CloseRequest;

                    if (Page == null)
                        Pages.Add(pageVM);
                    else
                    {
                        int indeksPage = Pages.IndexOf(Page);
                        Pages[indeksPage] = pageVM;
                    }
                    ActivePage = Page;

                    Page.CloseRequest += Page_CloseRequest;

                    if (Page is IPageReloadViewModel reload)
                        reload.Reload();
                    OnPropertyChanged(nameof(Page));
                    OnPropertyChanged(nameof(Pages));
                    OnPropertyChanged(nameof(ActivePage));
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

    private void Page_CloseRequest(object? sender, EventArgs e)
    {
        if(sender is IBasePageViewModel page && !page.IsMain)
        {
            Pages.Remove(page);
        }
    }

    #endregion
}

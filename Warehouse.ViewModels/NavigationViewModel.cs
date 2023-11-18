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
using Warehouse.ViewModels.Navigation;
using Warehouse.Models.Interfaces;

namespace Warehouse.ViewModel;

public class NavigationViewModel : BaseViewModel, INavigation
{
    #region Private fields

    private readonly Stack<IBasePageViewModel> _prevPages = new Stack<IBasePageViewModel>();
    private readonly Stack<IBasePageViewModel> _nextPages = new Stack<IBasePageViewModel>();
    private readonly IApp _app;
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

    //public IHost? AppHost { get; set; }

    public IBasePageViewModel Page
    {
        get => Pages?.FirstOrDefault(x => x.IsMain);
    }

    public IBasePageViewModel ActivePage { get; set; }

    public ObservableCollection<IBasePageViewModel> Pages { get; protected set; } = new ObservableCollection<IBasePageViewModel>();

    public event PageChanged PageChanged = (page) => { };

    public ObservableCollection<CustomMenuItem> MenuItems { get; protected set; } = new ObservableCollection<CustomMenuItem>();

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
        PrevCommand = new RelayCommand(SetPrevPage, () => CanSetPrevPage);
        NextCommand = new RelayCommand(SetNextPage, () => CanSetNextPage);
    }

    public NavigationViewModel(IApp app) : base()
    {
        _app = app;
        LoadMenu(); 
    }

    #endregion

    #region Private methods

    private void Page_CloseRequest(object? sender, EventArgs e)
    {
        if (sender is IBasePageViewModel page && !page.IsMain)
        {
            page.OnPageClose();

            if (!page.CanChangePage)
                return;
            Pages.Remove(page);
        }
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
                    _app.GetService<IApp>().ReloadDatabase();
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
            _app.GetService<IApp>().CatchExeption(ex);
            if (ex.PageVM == null)
                SetPage(ex.Page);
            else
                SetPage(ex.PageVM);
        }
        catch (Exception ex)
        {
            _app.GetService<IApp>().CatchExeption(ex);
            SetPage(prevPage);
        }
    }

    private void AddPrevPage(IBasePageViewModel page)
    {
        _prevPages.Push(page);
    }

    private void LoadMenu()
    {
        ObservableCollection<CustomMenuItem> menu = new ObservableCollection<CustomMenuItem>();

        menu.Add(new CustomMenuItem(this, EApplicationPage.DashBoard, Warehouse.Core.Properties.Resources.Home));
        IUser? user = _app.User;

        CustomMenuItem warehouseMenuitem = new CustomMenuItem(this, EApplicationPage.WareHouseCreator, Warehouse.Core.Properties.Resources.WareHouse)
        {
            ResourceIconName = "WareHouse",
            Items = new ObservableCollection<CustomMenuItem>()
        };

        if(user != null)
        {
            if (user.Type >= Models.Enums.EUserType.Employee_WareHouse)
            {
                menu.Add(new CustomMenuItem(this, EApplicationPage.Order, Warehouse.Core.Properties.Resources.Orders));
            }

            if (user.Type >= Models.Enums.EUserType.Employee_Office)
            {
                menu.Add(new CustomMenuItem(this, EApplicationPage.WareHouseItems, Warehouse.Core.Properties.Resources.WareHouseItems));
                menu.Add(new CustomMenuItem(this, EApplicationPage.Products, Warehouse.Core.Properties.Resources.Products)
                {
                    Items = new ObservableCollection<CustomMenuItem>()
                {
                    new CustomMenuItem(this,EApplicationPage.Suppliers, Warehouse.Core.Properties.Resources.Suppliers),
                    new CustomMenuItem(this,EApplicationPage.ProductGroups, Warehouse.Core.Properties.Resources.Group),
                    new CustomMenuItem(this,EApplicationPage.ProductStatuses, Warehouse.Core.Properties.Resources.Status),
                    new CustomMenuItem(this,EApplicationPage.ItemStates, Warehouse.Core.Properties.Resources.ItemState),
                }
                });

                warehouseMenuitem.Items.Add(new CustomMenuItem(this, EApplicationPage.StorageUnits, Warehouse.Core.Properties.Resources.StorageUnits));
                warehouseMenuitem.Items.Add(new CustomMenuItem(this, EApplicationPage.Racks, Warehouse.Core.Properties.Resources.Racks));
            }

            if (user.Type >= Models.Enums.EUserType.Employee_Technic)
            {
                warehouseMenuitem.Items.Add(new CustomMenuItem(this, EApplicationPage.WareHouseCreator, Warehouse.Core.Properties.Resources.Creator));
            }

            if (user.Type >= Models.Enums.EUserType.Boss)
            {

            }

            if (user.Type >= Models.Enums.EUserType.Admin)
            {

            }
        }

        
        menu.Add(warehouseMenuitem);

        menu.Add(new CustomMenuItem(this, EApplicationPage.Settings, Warehouse.Core.Properties.Resources.Settings));
        MenuItems = menu;
    }

    #endregion

    #region Public Methods

    public IBasePageViewModel ToBasePage(EApplicationPage page)
    {
        if (_app == null)
            Debugger.Break();

        switch (page)
        {
            case EApplicationPage.DashBoard:
                return _app.GetService<DashBoardPageViewModel>();
            case EApplicationPage.Products:
                return _app.GetService<ProductsPageViewModel>();
            case EApplicationPage.Suppliers:
                return _app.GetService<SuppliersPageViewModel>();
            case EApplicationPage.ProductGroups:
                return _app.GetService<ProductGroupsPageViewModel>();
            case EApplicationPage.Settings:
                return _app.GetService<SettingsPageViewModel>();
            case EApplicationPage.ProductStatuses:
                return _app.GetService<ProductStatusesPageViewModel>();
            case EApplicationPage.ItemStates:
                return _app.GetService<ItemStatesPageViewModel>();
            case EApplicationPage.WareHouseItems:
                return _app.GetService<WareHousePageViewModel>();
            case EApplicationPage.StorageUnits:
                return _app.GetService<StorageUnitsPageViewModel>();
            case EApplicationPage.WareHouseCreator:
                return _app.GetService<WareHouseCreatorPageViewModel>();
            case EApplicationPage.Racks:
                return _app.GetService<RacksPageViewModel>();
            case EApplicationPage.Order:
                return _app.GetService<OrdersPageViewModel>();
            default:
                Debugger.Break();
                return null;
        }
    }

    public void AddPage(IBasePageViewModel page)
    {
        var prevPage = Page;
        try
        {
            _app.ReloadDatabase();
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
            _app.CatchExeption(ex);
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

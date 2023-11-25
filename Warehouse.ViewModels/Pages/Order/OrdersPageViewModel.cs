using Warehouse.Models.Enums;
using Warehouse.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Service;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using Warehouse.Core.Helpers;

namespace Warehouse.ViewModel.Pages;

public class OrdersPageViewModel : BasePageViewModel
{
    #region Fields

    protected readonly IOrderService _service;
    protected ObservableCollection<OrderViewModel> _items;
    protected string _searchString;
    protected OrderViewModel _selectedItem;
    protected bool _canAddNew = true;

    #endregion

    #region Properties

    public ICollectionView Collection { get; private set; }

    public virtual ObservableCollection<OrderViewModel> Items
    {
        get => _items;
        protected set
        {
            SetProperty(ref _items, value, nameof(Items),
                () =>
                {
                    if (Collection != null)
                        Collection.Filter -= FilterCollection;
                    Collection = CollectionViewSource.GetDefaultView(value);
                    Collection.Filter += FilterCollection;
                }
            );
        }
    }

    public OrderViewModel SelectedItem
    {
        get => _selectedItem;
        set { SetProperty(ref _selectedItem, value, nameof(SelectedItem)); }
    }

    public virtual string SearchString
    {
        get => _searchString;
        set { SetProperty(ref _searchString, value, nameof(SearchString), () => Collection.Refresh()); }
    }

    public bool CanAddNew
    {
        get => _canAddNew;
        protected set { SetProperty(ref _canAddNew, value, nameof(CanAddNew)); }
    }

    #endregion

    #region Commands

    public ICommand AddItemCommand { get; private set; }

    public ICommand DeleteItemsCommand { get; private set; }

    public ICommand EditCommand { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="app">Application service</param>
    /// <param name="service">Service of product group item</param>
    public OrdersPageViewModel(
        IApp app, 
        IOrderService service) 
        : base(app)
    {
        _service = service;
        Page = Models.Page.EApplicationPage.Order;
        AddItemCommand = new AsyncRelayCommand(AddItem, canExecute: (o) => CanAddNew);
        DeleteItemsCommand = new AsyncRelayCommand<IList>(DeleteItems, canExecute: (o) => CanAddNew);
        EditCommand = new RelayCommand<OrderViewModel>(Edit);
    }

    #endregion

    #region Filter 

    private bool FilterCollection(object value)
    {
        if (value is OrderViewModel item && item != null && !string.IsNullOrEmpty(SearchString))
            return Filter(item, SearchString);
        else
            return true;
    }

    protected bool Filter(OrderViewModel value, string search)
    {
        if (value?.Name?.ToLower().Contains(search?.ToLower()) ?? true)
            return true;
        else
            return false;
    }

    #endregion

    #region Public methods

    public async override void OnPageOpen()
    {
        CanChangePage = false;
        Application.IsTaskRunning = true;
        List<Order> pgList = await _service.GetAllAsync();
        Items = new ObservableCollection<OrderViewModel>(pgList.Select(o => new OrderViewModel(_service, o, Application)));
        CanChangePage = true;

        CanAddNew = Application.User.Type >= EUserType.Employee_Office;

        Application.IsTaskRunning = false;
    }

    #endregion

    #region Command Methods

    private void Edit(OrderViewModel item)
    {
        try
        {
            if (item == null)
                return;
            OrderEditAddPageViewModel orderPage = new OrderEditAddPageViewModel(Application, item.Get());
            Application.Navigation.AddPage(orderPage);
        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    public async Task AddItem()
    {
        try
        {
            bool flag = true;
            if (SelectedItem != null)
                flag = await SelectedItem.SaveAsync();

            if (!flag)
            {
                Application.ShowSilentMessage(Warehouse.Core.Properties.Resources.ErrorWhileSaving);
                return;
            }

            OrderEditAddPageViewModel orderPage = new OrderEditAddPageViewModel(Application, null);

            Application.Navigation.AddPage(orderPage);


        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);
        }
        finally { Application.IsTaskRunning = false; }
    }

    public async Task DeleteItems(IList items)
    {
        if (items == null)
            return;
        try
        {  
            
            if (MessageBox.Show($"{Core.Properties.Resources.AreYouSure} ?", Core.Properties.Resources.Question, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IsTaskRunning = true;
                bool flag = true;
                List<Order> index = new List<Order>();
                foreach (Order pg in items)
                {
                    if (await _service.DeleteAsync(pg.ID))
                        index.Add(pg);
                    else
                    {
                        flag = false;
                    }
                    
                };
                index.ForEach(o => Items.Remove(Items.FirstOrDefault(x => x.ID == o.ID)));
                flag = await _service.SaveAsync();
                IsTaskRunning = false;
                if (!flag)
                {
                    if(index.Count == 0)
                        Application.ShowSilentMessage(Core.Properties.Resources.FailedToRemove, EMessageType.Warning);
                    else
                        Application.ShowSilentMessage($"{Core.Properties.Resources.FailedToRemove} {items.Count - index.Count} z {items.Count} {Core.Properties.Resources.FailedToRemove.ToLower()}", EMessageType.Warning);
                }
                else
                {
                    Application.ShowSilentMessage(Core.Properties.Resources.FailedToRemove, EMessageType.Ok);
                    _selectedItem = null;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }
        catch (Exception ex)
        {
            Application.ShowSilentMessage(ex.Message);
        }
        finally
        {
            IsTaskRunning = false;
        }
    }

    #endregion
}

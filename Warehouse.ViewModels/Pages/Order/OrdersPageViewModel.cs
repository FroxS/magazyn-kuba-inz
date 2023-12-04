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

public class OrdersPageViewModel : BasePageSearchItemsViewModel<OrderViewModel>
{
    #region Fields

    protected readonly IOrderService _service;
    private EOrderType _type = EOrderType.WareHouse;

    #endregion

    #region Properties

    public EOrderType Type
    {
        get => _type;
        set { SetProperty(ref _type, value); }
    }

    #endregion

    #region Commands

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

    protected override bool Filter(OrderViewModel value, string search)
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
        List<Order> pgList = await _service.GetAllAsync(Type);
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
            if (!_service.Save())
            {
                Application.ShowSilentMessage(Warehouse.Core.Properties.Resources.FailedToSave);
                return;
            }
            if (item == null)
                return;
            Order order = item.Get();
            order.Type = Type;
            Application.Navigation.OpenOrder(order);
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
                Application.ShowSilentMessage(Core.Properties.Resources.ErrorWhileSaving);
                return;
            }
            OrderEditAddPageViewModel orderPage = new OrderEditAddPageViewModel(Application, null,Type);
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
            if (Application.GetDialogService().AskUser($"{Core.Properties.Resources.AreYouSure} ?") == EDialogResult.Yes)
            {
                IsTaskRunning = true;
                bool flag = true;
                List<OrderViewModel> index = new List<OrderViewModel>();
                foreach (OrderViewModel pg in items)
                {
                    if (Application.Navigation.ExistOpenedOrder(pg.ID))
                    {
                        Application.ShowSilentMessage($"{Core.Properties.Resources.OrderIsOpened} : {pg.Name}", EMessageType.Warning);
                        return;
                    }
                    if (await _service.DeleteAsync(pg.ID))
                        index.Add(pg);
                    else
                    {
                        if (Application.GetDialogService().AskUser($"Nie można usunąć zamówienia {pg.Name}. Kontynuować ?") != EDialogResult.Yes)
                            return;
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
                    Application.ShowSilentMessage(Core.Properties.Resources.SuccessfulRemoved, EMessageType.Ok);
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

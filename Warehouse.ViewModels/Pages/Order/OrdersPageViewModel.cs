using Warehouse.Models.Enums;
using Warehouse.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class OrdersPageViewModel : 
    BasePageWIthItemsViewModel<
        Order,
        OrderViewModel,
        IOrderService>
{

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="app">Application service</param>
    /// <param name="service">Service of product group item</param>
    public OrdersPageViewModel(
        IApp app, 
        IOrderService service) 
        : base(app, service)
    {
        Page = Models.Page.EApplicationPage.Order;
        
    }

    #endregion

    #region Filter 

    protected override bool Filter(Order value, string search)
    {
        if (value?.Name?.ToLower().Contains(search?.ToLower()) ?? true)
            return true;
        else
            return false;
    }

    #endregion

    #region Public methods

    public override OrderViewModel GetVM(ref Order? item, OrderViewModel? lastVm = null)
    {
        if (item == null)
            return lastVm;
        bool flag = false;
        if (lastVm != null && !(lastVm?.Saved ?? true))
        {
            string? message = lastVm.Valid();
            if (message != null)
            {
                Application.ShowSilentMessage($"{message}", EMessageType.Warning);
                return lastVm;
            }
            ;

            var task = Task.Run(async () => await lastVm.SaveAsync());
            Task.WaitAll(task);
            if (!task.Result)
            {
                Application.ShowSilentMessage($"Nie udało się zapisać danych", EMessageType.Warning);
                return lastVm;
            }
        }
        OrderViewModel newVM = new OrderViewModel(
            _service, 
            item,
            Application);
        return newVM;
    }

    public async override void OnPageOpen()
    {
        CanChangePage = false;
        Application.IsTaskRunning = true;
        var selectedGuid = SelectedItemViewModel?.ID;
        _selectedItemViewModel = null;
        List<Order> pgList = await _service.GetAllAsync();
        Items = new ObservableCollection<Order>(pgList);
        if (selectedGuid.HasValue)
            SelectedItem = Items.FirstOrDefault(x => x.ID == selectedGuid.Value);
        else
            SelectedItem = Items.FirstOrDefault();
        CanChangePage = true;

        CanAddNew = Application.User.Type >= EUserType.Employee_Office;

        OnPropertyChanged(nameof(Items));
        OnPropertyChanged(nameof(SelectedItemViewModel));
        Application.IsTaskRunning = false;
    }

    #endregion

    #region Command Methods

    public override async Task AddItem()
    {
        try
        {
            bool flag = true;
            if (SelectedItemViewModel != null)
                flag = await SelectedItemViewModel.SaveAsync();

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

    public override async Task DeleteItems(IList items)
    {
        if (items == null)
            return;
        try
        {  
            
            if (MessageBox.Show($"{Warehouse.Core.Properties.Resources.AreYouSure} ?", Warehouse.Core.Properties.Resources.Question, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
                index.ForEach(o => Items.Remove(o));
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
                    _selectedItemViewModel = null;
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

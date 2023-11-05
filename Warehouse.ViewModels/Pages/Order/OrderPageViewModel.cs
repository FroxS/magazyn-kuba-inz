using Warehouse.Models.Enums;
using Warehouse.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class OrderPageViewModel : 
    BasePageWIthItemsViewModel<
        Order,
        OrderViewModel,
        IOrderService>
{
    #region Private fields


    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="app">Application service</param>
    /// <param name="service">Service of product group item</param>
    public OrderPageViewModel(
        IApp app, 
        IOrderService service) 
        : base(app, service)
    {
        Page = Models.Page.EApplicationPage.Order;
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
        try
        {
            CanChangePage = false;
            Application.IsTaskRunning = true;
            var selectedGuid = SelectedItemViewModel?.ID;
            _selectedItemViewModel = null;
            List<Order> pgList = await _service.GetAllAsync();
            Items = new ObservableCollection<Order>(pgList);
            if (selectedGuid.HasValue)
            {
                SelectedItem = Items.FirstOrDefault(x => x.ID == selectedGuid.Value);
            }
            CanChangePage = true;
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(SelectedItemViewModel));
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
        finally { Application.IsTaskRunning = false; }
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
                Application.ShowSilentMessage($"Nie można zapisać");
                return;
            }
            IsTaskRunning = true;
            string newName = _service.GetNewOrderName();
            var newvm = new Order() { Name = newName, RealizationDate = DateTime.Now.AddDays(10), ID_User = Application.User.ID };
            await _service.AddAsync(newvm);
            await _service.SaveAsync();
            Application.GetDispather().Invoke(() => { Items.Add(newvm); });
            SelectedItem = newvm;
            OnPropertyChanged(nameof(SelectedItemViewModel));
            Application.ShowSilentMessage($"Udało się dodać nową ofertę {_selectedItemViewModel.Name}", EMessageType.Ok);
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
            //if(items.Cast<Order>().Any(x => x.Reserved))
            //{
            //    Application.ShowSilentMessage("Zamównienie jest już zarezerwowane");
            //    return;
            //}    
            if (MessageBox.Show("Czy jesteś pewien ?","Pytanie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
                        Application.ShowSilentMessage("Nie udało się usunąć", EMessageType.Warning);
                    else
                        Application.ShowSilentMessage($"Nie udało się usunąć {items.Count - index.Count} z {items.Count} elementów", EMessageType.Warning);
                }
                else
                {
                    Application.ShowSilentMessage("Udało się usunąć", EMessageType.Ok);
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

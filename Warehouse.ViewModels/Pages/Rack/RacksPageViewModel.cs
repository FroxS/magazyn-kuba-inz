using Warehouse.Models.Enums;
using Warehouse.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class RacksPageViewModel : 
    BasePageWIthItemsViewModel<
        Rack, 
        RackViewModel, 
        IRackService>
{

    #region Constructors
    public RacksPageViewModel(IApp app, IRackService service) : base(app, service)
    {
    }

    #endregion

    #region Public methods

    public override RackViewModel GetVM(ref Rack? item, RackViewModel? lastVm = null)
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
        RackViewModel newVM = new RackViewModel(_service, item);
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
            List<Rack> pgList = await _service.GetAllAsync();
            Items = new ObservableCollection<Rack>(pgList);
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

    public override async Task DeleteItems(IList items)
    {
        if (items == null)
            return;
        try
        {
            if (MessageBox.Show("Czy jesteś pewien ?", "Pytanie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                bool flag = true;
                List<Rack> itemstoRemove = new List<Rack>();
                foreach (Rack item in items)
                {
                    flag = await _service.DeleteAsync(item.ID);
                    itemstoRemove.Add(item);
                    
                };
                itemstoRemove.ForEach(o => Items.Remove(o));
                flag = await _service.SaveAsync();

                if (!flag)
                {
                    Application.ShowSilentMessage("Nie udało się usunąć", EMessageType.Warning);
                }
                else
                {
                    Application.ShowSilentMessage("Udało się usunąć", EMessageType.Ok);
                    _selectedItemViewModel = null;
                    OnPropertyChanged(nameof(SelectedItemViewModel));
                }
            }
        }
        catch (Exception ex)
        {
            Application.ShowSilentMessage(ex.Message);
        }
    }

    #endregion
}

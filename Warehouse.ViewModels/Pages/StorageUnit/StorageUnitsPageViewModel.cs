using Warehouse.Models.Enums;
using Warehouse.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class StorageUnitsPageViewModel : 
    BasePageWIthItemsViewModel<
        StorageUnit,
        StorageUnitViewModel,
        IStorageUnitService>
{
    #region Constructors
    public StorageUnitsPageViewModel(IApp app, IStorageUnitService service) : base(app, service)
    {
    }

    #endregion

    #region Public methods

    public override StorageUnitViewModel GetVM(ref StorageUnit? item, StorageUnitViewModel? lastVm = null)
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
        StorageUnitViewModel newVM = new StorageUnitViewModel(_service, item);
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
            List<StorageUnit> pgList = await _service.GetAllAsync();
            Items = new ObservableCollection<StorageUnit>(pgList);
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
            StorageUnit item = await Application.GetInnerDialogService().AddStorageUnitInnerDialog();
            if (item == null)
                return;

            await _service.AddAsync(item);
            await _service.SaveAsync();
            Application.GetDispather().Invoke(() => { Items.Add(item); });
            SelectedItem = item;
            OnPropertyChanged(nameof(SelectedItemViewModel));
            Application.ShowSilentMessage($"Udało się dodać jednostkę magazynową {_selectedItemViewModel.Name}", EMessageType.Ok);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally { Application.IsTaskRunning = false; }
    }

    public override async Task DeleteItems(IList items)
    {
        if (items == null)
            return;
        try
        {
            if (MessageBox.Show("Czy jesteś pewien ?", "Pytanie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                bool flag = true;
                List<StorageUnit> itemstoRemove = new List<StorageUnit>();
                foreach (StorageUnit item in items)
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
            Application.ShowSilentMessage("Błąd podczas usuwania");
        }
    }


    #endregion
}

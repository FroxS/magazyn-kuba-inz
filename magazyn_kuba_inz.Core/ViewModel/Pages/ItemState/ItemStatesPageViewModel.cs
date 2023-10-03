using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Models.Enums;
using magazyn_kuba_inz.Models.WareHouse;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;

namespace magazyn_kuba_inz.Core.ViewModel.Pages;

public class ItemStatesPageViewModel : 
    BasePageWIthItemsViewModel<
        ItemState, 
        ItemStateViewModel, 
        IItemStateService>
{
    #region Constructors
    public ItemStatesPageViewModel(IApp app, IItemStateService service) : base(app, service)
    {
    }

    #endregion

    #region Public methods

    public override ItemStateViewModel GetVM(ref ItemState? item, ItemStateViewModel? lastVm = null)
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
        ItemStateViewModel newVM = new ItemStateViewModel(_service, item);
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
            List<ItemState> pgList = await _service.GetAllAsync();
            Items = new ObservableCollection<ItemState>(pgList);
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
            Application.GetInnerDialogService().AddItemStateInnerDialog(async (o) =>
            {
                if (o == null)
                    return;

                await _service.AddAsync(o);
                await _service.SaveAsync();
                Application.GetDispather().Invoke(() => { Items.Add(o); });
                SelectedItem = o;
                OnPropertyChanged(nameof(SelectedItemViewModel));
                Application.ShowSilentMessage($"Udało się dodać stan {_selectedItemViewModel.Name}", EMessageType.Ok);
            });
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
                List<ItemState> itemstoRemove = new List<ItemState>();
                foreach (ItemState item in items)
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

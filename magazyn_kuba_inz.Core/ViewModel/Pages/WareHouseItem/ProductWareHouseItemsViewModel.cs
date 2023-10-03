using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.Enums;
using magazyn_kuba_inz.Models.WareHouse;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel.Pages;

public class ProductWareHouseItemsViewModel : BaseViewModel
{
    #region Private fields

    private readonly IApp _app;

    private readonly IWareHouseItemService _service;

    private ObservableCollection<WareHouseStatusTabViewModel> items;

    private WareHouseStatusTabViewModel _selectedItem;

    private bool _hasItems;

    private bool canAssignNewStatus = false;

    #endregion

    #region Public Properties

    public Product Product { get; }

    public string Name => Product.Name ?? "";

    public ObservableCollection<WareHouseStatusTabViewModel> Items
    {
        get => items;
        private set
        {
            items = value;
            OnPropertyChanged(nameof(Items));
        }
    }

    public WareHouseStatusTabViewModel SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem == value)
                return;
            _selectedItem = value;
            if (_selectedItem is ITab tab)
                tab.Load();
            OnPropertyChanged(nameof(SelectedItem));
        }
    }

    public bool HasItems => (Items?.Count ?? 0) > 0;

    public bool CanAssignNewStatus 
    { 
        get => canAssignNewStatus;
        set{
            canAssignNewStatus = value;
            OnPropertyChanged(nameof(CanAssignNewStatus));
        }
    }

    #endregion

    #region Commands

    public ICommand AddProductToStateCommand { get; set; }

    #endregion

    #region Constructors

    public ProductWareHouseItemsViewModel(IWareHouseItemService service, Product product, IApp app) : base()
    {
        _service = service;
        _app = app;
        Product = product;
        AddProductToStateCommand = new AsyncRelayCommand(AddProductToState);
        LoadDataAsync();
    }

    #endregion

    #region Private methods

    private async void LoadDataAsync()
    {
        IsTaskRunning = true;
        Items = new ObservableCollection<WareHouseStatusTabViewModel>(
            (await _service.GetAllWareHouseItemToViewModel(Product.ID)).Select(x => new WareHouseStatusTabViewModel(_service, x.Get()))
        );
        Items.ToList().ForEach(x => x.OnItemCountChange += Vm_OnItemCountChange);
        SelectedItem = Items.FirstOrDefault();
        OnPropertyChanged(nameof(HasItems));
        
        var ItemStateservice = _app.GetService<IItemStateService, ItemState>();
        CanAssignNewStatus = Items.Count < (await ItemStateservice.GetAllAsync()).Count;
        IsTaskRunning = false;
    }

    #endregion

    #region Command Methods

    private async Task AddProductToState()
    {
        try
        {
            var service = _app.GetService<IItemStateService, ItemState>();
            var items = await service.GetAllAsync();
            List<ItemState> left = new List<ItemState>();

            foreach (var state in items)
            {
                var found = Items.FirstOrDefault(x => x.State.ID == state.ID);
                if (found == null)
                    left.Add(state);
            }
             
            if (left.Count == 0)
            {
                CanAssignNewStatus = false;
                return;
            }
                
            //var left = items.Where(x => Items.FirstOrDefault(x => x.State.ID == x.ID) == null).ToList();
            _app.GetInnerDialogService().AddProductToStateInnerDialog(
                Product,
                left,
                async (o) =>
                {
                    if (o == null)
                        return;

                    try
                    {
                        await _service.AddAsync(o);
                        //_service.Update(o);
                        bool flag = await _service.SaveAsync();
                        if (!flag)
                        {
                            _app.ShowSilentMessage($"Nie udało się dodać", EMessageType.Error);
                        }
                        else
                        {
                            
                            var vm = new WareHouseStatusTabViewModel(_service, await _service.GetByIdAsync(o.ID));
                            _app.GetDispather().Invoke(() => { Items.Add(vm); SelectedItem = vm; });
                            //Items.Add(vm); SelectedItem = vm;
                            vm.OnItemCountChange += Vm_OnItemCountChange;
                            CanAssignNewStatus = Items.Count < (await service.GetAllAsync()).Count;
                        }
                    }
                    catch
                    {
                        _app.ShowSilentMessage($"Nie udało się dodać", EMessageType.Error);
                    }
                }
                );
        }
        catch (Exception ex)
        {

        }
    }

    private void Vm_OnItemCountChange(WareHouseItem newItem, WareHouseItem oldItem)
    {
        if(newItem == null)
        {
            var found = Items.FirstOrDefault(x => x.State == oldItem.State);
            if(found != null)
            {
                Items.Remove(found);
                SelectedItem = Items.FirstOrDefault();
                var service = _app.GetService<IItemStateService, ItemState>();
                CanAssignNewStatus = Items.Count < (service.GetAll()).Count;
            }
        }
    }

    #endregion
}

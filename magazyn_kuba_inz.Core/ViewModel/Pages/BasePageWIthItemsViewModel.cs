using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.Enums;
using magazyn_kuba_inz.Models.WareHouse;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel.Pages;

public abstract class BasePageWIthItemsViewModel<Item, ItemViewModel, ItemService> : BasePageViewModel 
    where Item: BaseEntity 
    where ItemViewModel : BaseEntityViewModel<Item>
    where ItemService : IBaseService<Item>
{
    #region Private fields

    protected readonly ItemService _service;
    protected ObservableCollection<Item> _items;
    protected Item _selectedItem;
    protected ItemViewModel _selectedItemViewModel;

    #endregion

    #region Public Properties

    public virtual ObservableCollection<Item> Items
    {
        get => _items;
        protected set { _items = value; OnPropertyChanged(nameof(Items)); }
    }

    public virtual Item? SelectedItem
    {
        get => _selectedItem;
        set {
            var vm =  GetVM(ref value, _selectedItemViewModel);
            if(_selectedItemViewModel != vm)
            {
                _selectedItem = value;
                SelectedItemViewModel = vm;
            }
            OnPropertyChanged(nameof(SelectedItem));
            
        }
    }

    public virtual ItemViewModel? SelectedItemViewModel
    {
        get => _selectedItemViewModel;
        protected set { 
            _selectedItemViewModel = value; 
            OnPropertyChanged(nameof(SelectedItemViewModel)); 
        }
    }
    //public virtual ItemViewModel SelectedItemViewModel
    //{
    //    get => _selectedItemViewModel;
    //    set 
    //    {
    //        if (_selectedItemViewModel != null && _selectedItemViewModel.ID == value?.ID)
    //            return;

    //        if (_selectedItemViewModel != null)
    //        {
    //            var message = _selectedItemViewModel.Valid();
    //            if (message != null)
    //            {
    //                Application.ShowSilentMessage($"{message}", MessageType.Warning);
    //                return;
    //            }
    //            _selectedItemViewModel.Save();
    //            _selectedItemViewModel.Message = null;
    //        }
    //        (_selectedItemViewModel = value)?.SetEnabled();
    //        _selectedItemViewModel?.OnLoad();
    //        OnPropertyChanged(nameof(SelectedItemViewModel)); 
    //    }
    //}

    #endregion

    #region Command

    public ICommand AddItemCommand { get; private set; }

    public ICommand DeleteItemsCommand { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="app">Application</param>
    /// <param name="service">Service of current item</param>
    public BasePageWIthItemsViewModel(IApp app, ItemService service) : base(app)
    {
        AddItemCommand = new AsyncRelayCommand(AddItem);
        DeleteItemsCommand = new AsyncRelayCommand<IList>(DeleteItems);
        _service = service;
    }

    #endregion

    #region Private methods

    //protected virtual async void LoadItems()
    //{
    //    try
    //    {
    //        CanChangePage = false;
    //        Application.IsTaskRunning = true;
    //        _service.RefreshDbContext();
    //        var selectedGuid = SelectedItemViewModel?.ID;
    //        _selectedItemViewModel = null;
    //        List<Item> pgList = await _service.GetAllAsync();
    //        Items = new ObservableCollection<ItemViewModel>();
    //        if (pgList != null)
    //        {
    //            pgList.ForEach(o => {

    //                var vm = (ItemViewModel)Activator.CreateInstance(typeof(ItemViewModel), args: new object[] { _service, o });
    //                Items.Add(vm);
    //                if(selectedGuid.HasValue)
    //                {
    //                    if (o.ID == selectedGuid)
    //                        _selectedItemViewModel = vm;
    //                }
    //            });
    //        }
    //        CanChangePage = true;
    //        OnPropertyChanged(nameof(Items));
    //        OnPropertyChanged(nameof(SelectedItemViewModel));
    //    }
    //    catch(Exception ex) { MessageBox.Show(ex.Message); }
    //    finally { Application.IsTaskRunning = false; }

    //}

    #endregion

    #region Public methods

    public override void OnPageClose()
    {
        try
        {
            CanChangePage = true;
            if (_selectedItemViewModel == null)
                return;
            else
            {
                var message = _selectedItemViewModel.Valid();
                if(message != null)
                {
                    var result = MessageBox.Show($"{message}. kontynuować bez zapisywaia? ", "", MessageBoxButton.YesNo);
                    if(result != MessageBoxResult.Yes)
                    {
                        _service.Delete(_selectedItemViewModel.ID);
                        CanChangePage = false;
                        Application.ShowSilentMessage($"{message}");
                        
                    }
                    return;
                }
            }

            if (!(_service.Save()))
            {
                CanChangePage = false;
                Application.ShowSilentMessage($"Błąd podczas zapisywania");
            }

        }
        catch (Exception ex)
        {
            Application.ShowSilentMessage(ex.Message);
            CanChangePage = false;
        }
    }

    #endregion

    #region Abstract command Methods

    public abstract Task AddItem();

    public abstract Task DeleteItems(IList items);

    public abstract ItemViewModel GetVM(ref Item? item, ItemViewModel? lastVm = null);

    #endregion
}

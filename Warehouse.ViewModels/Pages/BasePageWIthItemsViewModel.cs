using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using Warehouse.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Warehouse.ViewModel.Service;
using Warehouse.Service.Interface;

namespace Warehouse.ViewModel.Pages;

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
    protected bool _canAddNew = true;
    protected bool _canDelete = true;

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

    public virtual bool CanAddNew
    {
        get => _canAddNew;
        protected set
        {
            _canAddNew = value;
            OnPropertyChanged(nameof(CanAddNew));
        }
    }

    public virtual bool CanDelete
    {
        get => _canDelete;
        protected set
        {
            _canDelete = value;
            OnPropertyChanged(nameof(CanDelete));
        }
    }

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
        AddItemCommand = new AsyncRelayCommand(AddItem, canExecute: (o) => CanAddNew);
        DeleteItemsCommand = new AsyncRelayCommand<IList>(DeleteItems, canExecute: (o) => CanAddNew);
        _service = service;
    }

    #endregion

    #region Private methods

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

    public virtual Task AddItem() { return null; }

    public abstract Task DeleteItems(IList items);

    public abstract ItemViewModel GetVM(ref Item? item, ItemViewModel? lastVm = null);

    #endregion
}

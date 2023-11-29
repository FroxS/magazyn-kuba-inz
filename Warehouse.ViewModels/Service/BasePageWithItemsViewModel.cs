using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using Warehouse.Models;
using System.Collections;
using System.Windows;
using System.Windows.Input;
using Warehouse.ViewModel.Service;

namespace Warehouse.ViewModel.Pages;

public abstract class BasePageWIthItemsViewModel<Item, ItemViewModel, ItemService> : BasePageSearchItemsViewModel<Item>, IPageReloadViewModel
    where Item: BaseEntity 
    where ItemViewModel : BaseEntityViewModel<Item>
    where ItemService : IBaseService<Item>
{
    #region Private fields

    protected readonly ItemService _service;
    protected ItemViewModel _selectedItemViewModel;

    #endregion

    #region Public Properties

    public override Item? SelectedItem
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

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="app">Application</param>
    /// <param name="service">Service of current item</param>
    public BasePageWIthItemsViewModel(IApp app, ItemService service) : this(app, service, null)
    {
        
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="app">Application</param>
    /// <param name="service">Service of current item</param>
    public BasePageWIthItemsViewModel(IApp app, ItemService service, BaseViewModel parent) : base(app, parent)
    {
        AddItemCommand = new AsyncRelayCommand(AddItem, canExecute: (o) => CanAddNew);
        DeleteItemsCommand = new AsyncRelayCommand<IList>(DeleteItems, canExecute: (o) => CanAddNew);
        _service = service;
    }



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

    public virtual void Reload()
    {
        _service.RefreshDbContext();
    }

    #endregion

    #region Abstract command Methods

    public virtual Task AddItem() { return null; }

    public abstract Task DeleteItems(IList items);

    public abstract ItemViewModel GetVM(ref Item? item, ItemViewModel? lastVm = null);

    #endregion
}

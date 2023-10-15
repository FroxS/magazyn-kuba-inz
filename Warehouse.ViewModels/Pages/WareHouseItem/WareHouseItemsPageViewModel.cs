using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.Collections.ObjectModel;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class WareHouseItemsPageViewModel : BasePageViewModel
{
    #region Private fields

    private readonly IWareHouseItemService _service;
    private readonly IProductService _productService;
    private ProductWareHouseItemsViewModel _wareHouseProductItemViewModel;
    private ObservableCollection<Product> _items;
    private Product? _selectedItem;

    #endregion

    #region Public properties

    public ObservableCollection<Product> Items
    {
        get => _items;
        private set
        {
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }

    public Product? SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            if (_selectedItem != null)
            {
                WareHouseProductItemViewModel = new ProductWareHouseItemsViewModel(_service, _selectedItem, Application);
            }
            OnPropertyChanged(nameof(SelectedItem));
        }
    }

    public ProductWareHouseItemsViewModel WareHouseProductItemViewModel
    {
        get => _wareHouseProductItemViewModel;
        set
        {
            _wareHouseProductItemViewModel = value;
            OnPropertyChanged(nameof(WareHouseProductItemViewModel));
        }
    }

    #endregion

    #region Constructors

    public WareHouseItemsPageViewModel(IApp app, IWareHouseItemService service, IProductService productService) : base(app)
    {
        _productService = productService;
        _service = service;
        Items = new ObservableCollection<Product>();
    }

    #endregion

    #region Private methods

    #endregion

    #region Command Methods

    #endregion

    #region Public Methods

    public async override void OnPageOpen()
    {
        try
        {
            CanChangePage = false;
            Application.IsTaskRunning = true;
            var selectedGuid = SelectedItem?.ID;
            _selectedItem = null;
            Items = new ObservableCollection<Product>(await _productService.GetAllAsync());
            SelectedItem = Items.FirstOrDefault(x => x.ID == selectedGuid);
            CanChangePage = true;
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(SelectedItem));
        }
        catch (Exception ex) { Application.GetDialogService().ShowAlert(ex.Message); }
        finally { Application.IsTaskRunning = false; }
    }

    #endregion
}

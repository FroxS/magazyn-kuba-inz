using System.Collections.ObjectModel;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.Windows.Input;
using Warehouse.Core.Helpers;

namespace Warehouse.ViewModel.Pages;

public class RacksPageViewModel : BasePageViewModel
{
    #region Private fields

    private readonly IRackService _service;
    private readonly IStorageItemPackageService _storageItemPackageService;
    private readonly IWareHouseItemService _wareHouseItemService;
    private readonly IStorageItemService _storageItemService;
    private ObservableCollection<Rack> _racks;
    private Rack _rack;
    private StorageItemPackage _selectedPackage;
    private ObservableCollection<StorageItem> _itemsInPackage = new ObservableCollection<StorageItem>();
    private ObservableCollection<WareHouseItem> _availableItems = new ObservableCollection<WareHouseItem>();
    private WareHouseItem _availableItem;
    private int _count;

    #endregion

    #region Public Properties

    public int Count
    {
        get => _count;
        set { SetProperty(ref _count, value, nameof(Count)); }
    }

    public ObservableCollection<Rack> Racks 
    {
        get => _racks; 
        set { SetProperty(ref _racks, value, nameof(Racks)); }
    }

    public Rack Rack
    {
        get => _rack;
        set { SetProperty(ref _rack, value, nameof(Rack)); }
    }

    public StorageItemPackage SelectedPackage
    {
        get => _selectedPackage;
        set {
            SetProperty(ref _selectedPackage, value, nameof(SelectedPackage),
            () => { if (value?.Items != null) ItemsInPackage = GetStoragrItemByPackageID(value.ID); });
             
        }
    }

    public ObservableCollection<StorageItem> ItemsInPackage
    {
        get => _itemsInPackage;
        set { SetProperty(ref _itemsInPackage, value, nameof(ItemsInPackage)); }
    }

    public ObservableCollection<WareHouseItem> AvailableItems
    {
        get => _availableItems;
        set { SetProperty(ref _availableItems, value, nameof(AvailableItems)); }
    }

    public WareHouseItem AvailableItem
    {
        get => _availableItem;
        set { SetProperty(ref _availableItem, value, nameof(AvailableItem)); }
    }

    #endregion

    #region Commands

    public ICommand AddNewPackageCommand { get; private set; }

    public ICommand AddToPackageCommand { get; private set; }

    public ICommand RemoveFromPackageCommand { get; private set; }

    #endregion

    #region Constructors

    public RacksPageViewModel(IApp app, 
        IRackService service, 
        IStorageItemPackageService storageItemPackageService,
        IStorageItemService storageItemService,
        IWareHouseItemService wareHouseItemService) : base(app)
    {
        _service = service;
        _storageItemPackageService = storageItemPackageService;
        _wareHouseItemService = wareHouseItemService;
        _storageItemService = storageItemService;
        AddNewPackageCommand = new RelayCommand(AddNewPackage, () => Rack != null);
        AddToPackageCommand = new RelayCommand(AddToPackage, () => Rack != null && SelectedPackage != null && AvailableItem != null);
        RemoveFromPackageCommand = new RelayCommand<StorageItem>(RemoveFromPackage, (o) => SelectedPackage != null && o != null);
    }


    #endregion

    #region Public methods

    public override void OnPageOpen()
    {
        Racks = new ObservableCollection<Rack>(_service.GetAllWithItems());
        Rack = Racks.FirstOrDefault();
        AvailableItems = new ObservableCollection<WareHouseItem>(_wareHouseItemService.GetProductsAvailableToRack());
    }

    #endregion

    #region Command Methods

    private void AddNewPackage()
    {
        try
        {
            StorageUnit unit = Application.GetDialogService().GetStorageUnit("Wybierz jednostkę");
            var package = new StorageItemPackage() { ID_StorageUnit = unit.ID, ID_Rack = Rack.ID };
            Rack.StorageItems.Add(package);
            _storageItemPackageService.Add(package);
            _storageItemPackageService.Save();
            
            SelectedPackage = package;

            OnPropertyChanged(nameof(Rack));

        }
        catch(Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    private void RemoveFromPackage(StorageItem item)
    {
        try
        {
            if (item == null)
                return;

            if (_wareHouseItemService.RemoveFromRack(item))
            {
                _wareHouseItemService.Save();
                _storageItemPackageService.Save();
                _storageItemService.Save();
                AvailableItems = new ObservableCollection<WareHouseItem>(_wareHouseItemService.GetProductsAvailableToRack());
                OnPropertyChanged(nameof(Rack));
                ItemsInPackage = GetStoragrItemByPackageID(SelectedPackage.ID);
                Application.ShowSilentMessage("Udało się", Models.Enums.EMessageType.Ok);
            }
            else
            {
                Application.ShowSilentMessage("Nie można usunać z paczki, prawdopowdomnie jest już ten produt w ofercie");
            }


        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);

        }
    }

    private void AddToPackage()
    {
        try
        {
            if (Rack == null || SelectedPackage == null || AvailableItem == null)
                return;
            var count = AvailableItem.Count <= Count ? AvailableItem.Count : Count;
            WareHouseItem curentProduct = AvailableItem;
            List<StorageItem> items = new List<StorageItem>();
            for (int i = 0; i < count; i++)
            {
                var item = new StorageItem()
                {
                    ID = Guid.NewGuid(),
                    ID_Item = AvailableItem.ID,
                    ID_Package = SelectedPackage.ID,
                    ID_OrderItem = null
                };
                items.Add(item);
            }
            if (_wareHouseItemService.MoveItemsToRack(items))
            {
                _wareHouseItemService.Save();
                AvailableItems = new ObservableCollection<WareHouseItem>(_wareHouseItemService.GetProductsAvailableToRack());
                OnPropertyChanged(nameof(Rack));
                ItemsInPackage = GetStoragrItemByPackageID(SelectedPackage.ID);
                Application.ShowSilentMessage("Udało się", Models.Enums.EMessageType.Ok);
            }
            else
            {
                Application.ShowSilentMessage("Nie można przenieć elementów na stojak");
            }
            

        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    private ObservableCollection<StorageItem> GetStoragrItemByPackageID(Guid id)
    {
        return new ObservableCollection<StorageItem>(_storageItemService.GetItemsByPackage(id));
    }

    #endregion
}

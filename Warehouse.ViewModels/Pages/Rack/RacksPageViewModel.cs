using System.Collections.ObjectModel;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.Windows.Input;
using Warehouse.Core.Helpers;
using Warehouse.Models.Enums;

namespace Warehouse.ViewModel.Pages;

public class RacksPageViewModel : BasePageViewModel
{
    #region Private fields

    private readonly IRackService _service;
    private readonly IStorageItemPackageService _storageItemPackageService;
    private readonly IWareHouseService _wareHouseItemService;
    private readonly IStorageItemService _storageItemService;
    private ObservableCollection<Rack> _racks;
    private Rack _rack;
    private StorageItemPackage _selectedPackage;
    private ObservableCollection<StorageItem> _itemsInPackage = new ObservableCollection<StorageItem>();
    private ObservableCollection<StorageItem> _availableItems = new ObservableCollection<StorageItem>();
    private StorageItem _selectedAvailableItem = null;

    #endregion

    #region Public Properties

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
        set { SetProperty(ref _selectedPackage, value, nameof(SelectedPackage), () => Load());}
    }

    public ObservableCollection<StorageItem> ItemsInPackage
    {
        get => _itemsInPackage;
        set { SetProperty(ref _itemsInPackage, value, nameof(ItemsInPackage)); }
    }

    public ObservableCollection<StorageItem> AvailableItems
    {
        get => _availableItems;
        set { SetProperty(ref _availableItems, value, nameof(AvailableItems)); }
    }

    public StorageItem SelectedAvailableItem
    {
        get => _selectedAvailableItem;
        set { SetProperty(ref _selectedAvailableItem, value, nameof(SelectedAvailableItem)); }
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
        IWareHouseService wareHouseItemService) : base(app)
    {
        Page = Models.Page.EApplicationPage.Racks;
        _service = service;
        _storageItemPackageService = storageItemPackageService;
        _wareHouseItemService = wareHouseItemService;
        _storageItemService = storageItemService;
        AddNewPackageCommand = new RelayCommand(AddNewPackage, () => Rack != null);
        AddToPackageCommand = new RelayCommand(AddToPackage, () => Rack != null && SelectedPackage != null);
        RemoveFromPackageCommand = new RelayCommand<StorageItem>(RemoveFromPackage, (o) => SelectedPackage != null && o != null);
    }


    #endregion

    #region Public methods

    public override void OnPageOpen()
    {
        Racks = new ObservableCollection<Rack>(_service.GetAllWithItems());
        Rack = Racks.FirstOrDefault();
        Load();
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
            Load();
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

            if (_wareHouseItemService.MoveProductToState(EState.Received , item) == null)
            {
                _wareHouseItemService.Save();
                _storageItemPackageService.Save();
                _storageItemService.Save();
                AvailableItems = new ObservableCollection<StorageItem>(_wareHouseItemService.GetProductsByState(EState.InStock));
                OnPropertyChanged(nameof(Rack));
                Load();
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
            if (Rack == null || SelectedPackage == null || SelectedAvailableItem == null)
                return;

            if(SelectedAvailableItem.State.State < EState.InStock)
            {
                EState currentState = SelectedAvailableItem.State.State;
                while(!(currentState == EState.InStock))
                {
                    currentState = (EState)((int)currentState << 1);
                    string? mess = _wareHouseItemService.MoveProductToState(currentState, SelectedAvailableItem);
                    if (mess != null)
                    {
                        Application.ShowSilentMessage($"{mess}");
                        return;
                    }
                }
            }

            SelectedAvailableItem.ID_Package = SelectedPackage.ID;
            string? message = _wareHouseItemService.MoveProductToState(EState.Available, SelectedAvailableItem);
            if(message == null && _wareHouseItemService.Save())
            {
                Application.ShowSilentMessage("Udało się", Models.Enums.EMessageType.Ok);
            }
            else
            {
                Application.ShowSilentMessage($"Nie można przenieć elementów na stojak: {message}");
            }
            Load();
        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    public void Load()
    {
        AvailableItems = new ObservableCollection<StorageItem>(_wareHouseItemService.GetProductsByState(EState.InStock | EState.Received));
        if(Rack != null)
        {
            if(SelectedPackage != null)
            {
                ItemsInPackage = new ObservableCollection<StorageItem>(_wareHouseItemService.GetItemsByPackage(SelectedPackage.ID) ?? new List<StorageItem>());
            }
        }
    }

    #endregion
}

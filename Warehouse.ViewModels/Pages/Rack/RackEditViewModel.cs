using Warehouse.ViewModel.Service;
using Warehouse.Models;
using Warehouse.Core.Interface;
using System.Collections.ObjectModel;
using Warehouse.Models.Enums;
using System.Windows.Input;
using Warehouse.Core.Helpers;

namespace Warehouse.ViewModel.Pages;

public class RackEditViewModel : BasePageViewModel, ISingleCardPage
{
    #region Private fields

    private Guid _rackID;
    private Rack? _rack;
    private string _selectedPackageName = "";
    private StorageItemPackage? _selectedPackage = null;
    private StorageItem? _selectedAvailableItem = null;
    private ObservableCollection<StorageItem> _itemsInPackage = new ObservableCollection<StorageItem>();
    private ObservableCollection<StorageItem> _availableItems = new ObservableCollection<StorageItem>();
    private ObservableCollection<StorageItemPackage> _itemsInRack = new ObservableCollection<StorageItemPackage>();

    #endregion

    #region Services

    private IWareHouseService _wareHouseItemService;
    private IStorageItemPackageService _storageItemPackageService;
    private IRackService _rackService;

    #endregion

    #region Public Properties

    public StorageItemPackage? SelectedPackage
    {
        get => _selectedPackage;
        set { SetProperty(ref _selectedPackage, value, onChanged: () => LoadItems()); }
    }

    public StorageItem? SelectedAvailableItem
    {
        get => _selectedAvailableItem;
        set { SetProperty(ref _selectedAvailableItem, value); }
    }

    public string SelectedPackageName
    {
        get => _selectedPackageName;
        set { SetProperty(ref _selectedPackageName, value); }
    }

    public ObservableCollection<StorageItem> ItemsInPackage
    {
        get => _itemsInPackage;
        set { SetProperty(ref _itemsInPackage, value); }
    }

    public ObservableCollection<StorageItem> AvailableItems
    {
        get => _availableItems;
        set { SetProperty(ref _availableItems, value); }
    }

    public Rack? Rack
    {
        get => _rack;
        protected set => SetProperty(ref _rack, value, onChanged:() => {
            if (value != null)
                ItemsInRack = new ObservableCollection<StorageItemPackage>(_rackService.GetAllPackages(value.ID) ?? new List<StorageItemPackage>());
            else
                ItemsInRack = new ObservableCollection<StorageItemPackage>();
        });
    }

    public ObservableCollection<StorageItemPackage> ItemsInRack
    {
        get => _itemsInRack;
        protected set { SetProperty(ref _itemsInRack, value); }
    }

    #endregion

    #region Commands

    public ICommand AddNewPackageCommand { get; protected set; }

    public ICommand AddToPackageCommand { get; protected set; }

    public ICommand RemoveFromPackageCommand { get; protected set; }

    public ICommand RemovePackageCommand { get; protected set; }

    public ICommand ChangeRackFlorCommand { get; protected set; }

    #endregion

    #region Constructors

    public RackEditViewModel(IApp app) : this(Guid.Empty,app)
    {
        
    }

    public RackEditViewModel(Guid rackid, IApp app) : base(app)
    {
        Page = Models.Page.EApplicationPage.Rack;
        _rackID = rackid;
        AddNewPackageCommand = new RelayCommand(AddNewPackage, () => Rack != null);
        AddToPackageCommand = new RelayCommand(AddToPackage, () => SelectedPackage != null && SelectedAvailableItem != null);
        RemoveFromPackageCommand = new RelayCommand<StorageItem>(RemoveFromPackage, (o) => SelectedPackage != null && o != null);
        RemovePackageCommand = new RelayCommand(RemovePackage, () => SelectedPackage != null);
        ChangeRackFlorCommand = new RelayCommand(() => ChangeRackFlor(), () => Rack != null && SelectedPackage != null);
        OnPageOpen();
        if(Application?.MainWindow !=null)
            Application.MainWindow.KeyDown += MainWindow_KeyDown;
    }


    #endregion

    #region Events

    private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        try
        {
            if (e.Key == Key.Add && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (Rack == null || SelectedPackage == null || SelectedAvailableItem == null)
                    return;

                AddToPackage();
                SelectedAvailableItem = AvailableItems.FirstOrDefault();
            }


            if (e.Key == Key.Up && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (Rack == null || SelectedPackage == null)
                    return;
                ChangeRackFlor(1);
            }

            if (e.Key == Key.Down && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (Rack == null || SelectedPackage == null)
                    return;
                ChangeRackFlor(-1);
            }
        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    #endregion

    #region Load

    public override void OnPageOpen()
    {
        _wareHouseItemService = Application.GetService<IWareHouseService>();
        _storageItemPackageService = Application.GetService<IStorageItemPackageService>();
        _rackService = Application.GetService<IRackService>();
        ReloadRack();
        LoadItems();
    }

    public void ChangeRack(Guid rackID)
    {
        _rackID = rackID;
        OnPageOpen();
        if (SelectedPackage != null && Rack?.StorageItems != null)
        {
            if (!Rack.StorageItems.Contains(SelectedPackage))
                SelectedPackage = null;
        }
    }

    #endregion

    #region Private methods

    private void LoadItems()
    {

        if (Rack == null)
            return;
        Title = Rack.GetName();

        SelectedPackageName = SelectedPackage == null ? "" : $"[{SelectedPackage?.Items?.Count ?? 0}] {Core.Properties.Resources.Flor} ({SelectedPackage?.Flor ?? 0}) ";

        AvailableItems = new ObservableCollection<StorageItem>(_wareHouseItemService.GetProductsByState(EState.InStock | EState.Received));
        if (SelectedPackage != null)
        {
            ItemsInPackage = new ObservableCollection<StorageItem>(_wareHouseItemService.GetItemsByPackage(SelectedPackage.ID) ?? new List<StorageItem>());
        }
        else
            ItemsInPackage = new ObservableCollection<StorageItem>();
    }

    #endregion

    #region Command Methods

    private void ChangeRackFlor(int plus = 1)
    {
        try
        {
            if (Rack == null || SelectedPackage == null)
                return;

            int naxtFlor = SelectedPackage.Flor + plus;

            if (Rack.Flors == naxtFlor)
                naxtFlor = 0;

            SelectedPackage.Flor = naxtFlor;
            _storageItemPackageService.Update(SelectedPackage);
            if (!_storageItemPackageService.Save())
                Application.ShowSilentMessage(Core.Properties.Resources.FailedToSave);

            ReloadRack();
            LoadItems();
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
            if (SelectedPackage == null || SelectedAvailableItem?.State == null)
                return;

            if (SelectedAvailableItem.State.State < EState.InStock)
            {
                EState currentState = SelectedAvailableItem.State.State;
                while (!(currentState == EState.InStock))
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
            if (!(message == null && _wareHouseItemService.Save()))
            {
                Application.ShowSilentMessage($"{Core.Properties.Resources.ErrorWhileAssigningProductsToRack}: {message}");
            }
            ReloadRack();
            LoadItems();
        }
        catch (Exception ex)
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

            if (_wareHouseItemService.MoveProductToState(EState.Received, item) == null)
            {
                if(_wareHouseItemService.Save())
                    AvailableItems = new ObservableCollection<StorageItem>(_wareHouseItemService.GetProductsByState(EState.InStock));
                else
                    Application.ShowSilentMessage(Core.Properties.Resources.FailedToSave);
                ReloadRack();
                LoadItems();
            }
            else
                Application.ShowSilentMessage(Core.Properties.Resources.CannotRemovedPackageProductProbablyInOffer);

        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);

        }
    }

    private void RemovePackage()
    {
        try
        {
            StorageItemPackage? package = SelectedPackage;
            if (package == null)
                return;


            if (Application.GetDialogService().AskUser(Core.Properties.Resources.AreYouSure + " ?", Core.Properties.Resources.Question) != EDialogResult.Yes)
                return;

            if ((package?.Items?.Count ?? 0) > 0)
            {
                if (Application.GetDialogService().AskUser($"{Core.Properties.Resources.PackageContainsProductsRemoveThemFromRack} ?") == EDialogResult.Yes)
                {
                    string? message = _wareHouseItemService.MoveProductToState(EState.InStock, package.Items.ToArray());
                    if (message != null)
                    {
                        Application.ShowSilentMessage($"{Core.Properties.Resources.ErrorDuringMovingItems}: {message}");
                        return;
                    }
                }
                else
                    return;
            }

            if (!(_storageItemPackageService.Delete(package.ID) && _storageItemPackageService.Save()))
            {
                Application.ShowSilentMessage($"{Core.Properties.Resources.FailedToRemove}");
            }
            ReloadRack();
            LoadItems();
        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);

        }
    }

    private void AddNewPackage()
    {
        try
        {
            StorageUnit unit = Application.GetDialogService().GetStorageUnit(Core.Properties.Resources.SelectUnit);
            StorageItemPackage package = new StorageItemPackage() { ID_StorageUnit = unit.ID, ID_Rack = _rack.ID };
            _rack.StorageItems.Add(package);
            _storageItemPackageService.Add(package);
            _storageItemPackageService.Save();

            SelectedPackage = package;
            ReloadRack();
            LoadItems();
            
        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    private void ReloadRack()
    {
        Rack = _rackID == Guid.Empty ? null : _rackService.GetById(_rackID);
    }

    public bool ExistPage(IBasePageViewModel page)
    {
        if (page is RackEditViewModel rackEdit && rackEdit._rackID == _rackID)
            return true;


        return false;
    }


    #endregion

}

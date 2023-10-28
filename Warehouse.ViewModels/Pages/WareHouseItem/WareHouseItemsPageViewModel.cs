using Warehouse.ViewModel.Service;
using System.Collections.ObjectModel;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class WareHouseItemsPageViewModel : BasePageViewModel
{
    #region Private fields

    private readonly IWareHouseItemService _service;
    private readonly IItemStateService _itemStateService;
    private ObservableCollection<ProtuctStateTabViewModel> _states;
    private ProtuctStateTabViewModel? _selectedState;

    #endregion

    #region Public properties

    public ObservableCollection<ProtuctStateTabViewModel> States
    {
        get => _states;
        private set
        {
            _states = value;
            OnPropertyChanged(nameof(States));
        }
    }

    public ProtuctStateTabViewModel? SelectedState
    {
        get => _selectedState;
        set
        {
            _selectedState = value;
            if (_selectedState != null)
                _selectedState.Load();
            OnPropertyChanged(nameof(SelectedState));
        }
    }

    #endregion

    #region Constructors

    public WareHouseItemsPageViewModel(IApp app, IItemStateService itemStateService, IWareHouseItemService service, IProductService productService) : base(app)
    {
        Page = Models.Page.EApplicationPage.WareHouseItems;
        _itemStateService = itemStateService;
        _service = service;
    }

    #endregion

    #region Public Methods

    public async override void OnPageOpen()
    {
        try
        {
            CanChangePage = false;
            Application.IsTaskRunning = true;
            States = new ObservableCollection<ProtuctStateTabViewModel>(_itemStateService.GetAll().OrderBy(x => x.State).Select(
                s => 
                new ProtuctStateTabViewModel(_service, _itemStateService, Application, s)
                ));
            SelectedState = States.FirstOrDefault();
            CanChangePage = true;
        }
        catch (Exception ex) { Application.GetDialogService().ShowAlert(ex.Message); }
        finally { Application.IsTaskRunning = false; }
    }

    #endregion
}

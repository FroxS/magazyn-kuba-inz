using Warehouse.ViewModel.Service;
using System.Collections.ObjectModel;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class WareHousePageViewModel : BasePageViewModel
{
    #region Private fields

    protected readonly IWareHouseService _service;
    protected readonly IItemStateService _itemStateService;
    protected ObservableCollection<ProtuctStateTabViewModel> _states;
    protected ProtuctStateTabViewModel? _selectedState;

    #endregion

    #region Public properties

    public ObservableCollection<ProtuctStateTabViewModel> States
    {
        get => _states;
        protected set
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
                _selectedState.OnPageOpen();
            OnPropertyChanged(nameof(SelectedState));
        }
    }

    #endregion

    #region Constructors

    public WareHousePageViewModel(IApp app, IItemStateService itemStateService, IWareHouseService service) : base(app)
    {
        Page = Models.Page.EApplicationPage.WareHouseItems;
        _itemStateService = itemStateService;
        _service = service;
    }

    public WareHousePageViewModel() : base()
    {
        Page = Models.Page.EApplicationPage.WareHouseItems;
        
    }

    #endregion

    #region Public Methods

    public override void OnPageOpen()
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
        catch (Exception ex) { Application.CatchExeption(ex); }
        finally { Application.IsTaskRunning = false; }
    }

    #endregion
}

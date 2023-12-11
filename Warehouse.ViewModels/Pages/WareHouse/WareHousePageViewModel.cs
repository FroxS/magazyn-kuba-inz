using Warehouse.ViewModel.Service;
using System.Collections.ObjectModel;
using Warehouse.Core.Interface;
using Warehouse.Models;

namespace Warehouse.ViewModel.Pages;

public class WareHousePageViewModel : BasePageViewModel
{
    #region Private fields

    protected IWareHouseService _service => Application.GetService<IWareHouseService>();
    protected IItemStateService _itemStateService => Application.GetService<IItemStateService>();
    protected ObservableCollection<ProtuctStateTabViewModel> _states;
    protected ProtuctStateTabViewModel? _selectedState;

    #endregion

    #region Public properties

    public ObservableCollection<ProtuctStateTabViewModel> States
    {
        get => _states;
        protected set => SetProperty(ref _states, value);
    }

    public ProtuctStateTabViewModel? SelectedState
    {
        get => _selectedState;
        set
        {
            _selectedState?.OnPageClose();
            SetProperty(ref _selectedState, value, onChanged: () => value?.OnPageOpen());
        }
    }

    #endregion

    #region Constructors

    public WareHousePageViewModel(IApp app) : base(app)
    {
        Page = Models.Page.EApplicationPage.WareHouseItems;
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

            States = new ObservableCollection<ProtuctStateTabViewModel>();

			foreach (ItemState state in _itemStateService.GetAll().OrderBy(x => x.State))
            {
                if(state.State == Models.Enums.EState.Reserved)
                {
					States.Add(new ProtuctStateWithOrderTabViewModel(Application, state));
				}
                else
                {
					States.Add(new ProtuctStateTabViewModel(Application, state));
				}
			}
            SelectedState = States.FirstOrDefault();
            CanChangePage = true;
        }
        catch (Exception ex) { Application.CatchExeption(ex); }
        finally { Application.IsTaskRunning = false; }
    }

    #endregion
}

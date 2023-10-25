using Warehouse.Core.Helpers;
using Warehouse.Core.Models;
using Warehouse.ViewModel.Service;
using System.Windows.Input;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class WareHouseCreatorPageViewModel : BasePageViewModel
{
    #region Private fields

    private IHallService _hallService;
    private IRackService _rackService;
    private HallObject _hall;
    private IBaseObject _selectedObject;
    private Func<RackObject,bool> _canDeleteRack;

    #endregion

    #region Public properties

    public HallObject Hall
    {
        get => _hall;
        set { SetProperty(ref _hall, value, nameof(Hall)); }
    }

    public IBaseObject SelectedObject
    {
        get => _selectedObject;
        set { SetProperty(ref _selectedObject, value, nameof(SelectedObject)); 
        }
    }

    public Func<RackObject, bool> CanDeleteRack
    {
        get => _canDeleteRack;
        set { SetProperty(ref _canDeleteRack, value, nameof(CanDeleteRack)); }
    }

    #endregion

    #region Commands

    public ICommand AddRackCommand { get; protected set; }

    public ICommand RemoveRackCommand { get; protected set; }

    public ICommand EditHallCommand { get; protected set; }

    #endregion

    #region Constructors

    public WareHouseCreatorPageViewModel(IApp app, IHallService hallService, IRackService rackService) : base(app)
    {
        _hallService = hallService;
        _rackService = rackService;
        EditHallCommand = new AsyncRelayCommand(() => EditHall());
        CanDeleteRack = (rack) => {
            bool flag = rackService.CanDeleteRack(rack.ID);
            flag = false;
            if (!flag)
                Application.ShowSilentMessage("Nie można usunąć stojaka, Prawdopodobnie posiada jakieś elementy");

            return flag;
        };
    }

    #endregion

    #region Private methods

    public override void OnPageOpen()
    {
        base.OnPageOpen(); 
        var hall = _hallService.GetAll().FirstOrDefault();


        if(hall == null)
        {
            Application.GetInnerDialogService().GetHallInnerDialog((o) => {

                if (o == null)
                {
                    Application.Navigation.SetPage(Warehouse.Models.Page.EApplicationPage.DashBoard);
                    return;
                }
                var p1 = new WayPointObject(100, 100) { IsStartPoint = true };
                var p2 = new WayPointObject(200, 100);
                p1.AddConnection(ref p2);
                o.WayPoints.Add(p1);
                o.WayPoints.Add(p2);
                Hall = o;
            });
        }
        else
        {
            Hall = _hallService.GetHallObject(hall.ID);
        }
    }

    private async Task EditHall()
    {
        HallObject hall = await Application.GetInnerDialogService().GetHallInnerDialog(Hall);

        if (hall != null)
            Hall = hall;
    }

    public override void OnPageClose()
    {
        if (Hall == null)
            return;
        string? message = _hallService.IsHallOk(Hall);
        if (message != null)
        {
            Application.ShowSilentMessage(message);
            CanChangePage = false;
        }
        else
        {
            if (_hallService.Exist(Hall.Id))
            {
                CanChangePage = _hallService.UpdatehHllObject(Hall);
            }
            else
            {
                CanChangePage = _hallService.Add(_hallService.GetHall(Hall));
            }
            if (CanChangePage)
                _hallService.Save();
        } 
    }

    #endregion

    #region Command Methods

    
    #endregion

    #region Public Methods



    #endregion

    #region Private methods


    #endregion
}

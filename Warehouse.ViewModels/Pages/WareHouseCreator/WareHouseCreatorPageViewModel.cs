using Warehouse.Core.Helpers;
using Warehouse.Core.Models;
using Warehouse.ViewModel.Service;
using System.Windows.Input;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json;
using Warehouse.Models;
using System.Collections.ObjectModel;

namespace Warehouse.ViewModel.Pages;

public class WareHouseCreatorPageViewModel : BasePageViewModel
{
    #region Private fields

    private IHallService _hallService;
    private IRackService _rackService;
    private HallObject _hall;
    private bool _canEdit = false;
    private bool _toSave = false;
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
        set { SetProperty(ref _selectedObject, value, nameof(SelectedObject), 
            () =>
            {
                if(value != null && _selectedObject is RackObject ro)
                {
                    ro.Items = new ObservableCollection<StorageItem>(_rackService.GetAllPackages(ro.ID).SelectMany(x => x.Items));
                }
            }
            ); 
        }
    }

    public Func<RackObject, bool> CanDeleteRack
    {
        get => _canDeleteRack;
        set { SetProperty(ref _canDeleteRack, value, nameof(CanDeleteRack)); }
    }

    public bool CanEdit
    {
        get => _canEdit;
        set { SetProperty(ref _canEdit, value, nameof(CanEdit)); }
    }

    public bool ToSave
    {
        get => _toSave;
        set { SetProperty(ref _toSave, value, nameof(ToSave)); }
    }

    #endregion

    #region Commands

    public ICommand AddRackCommand { get; protected set; }

    public ICommand RemoveRackCommand { get; protected set; }

    public ICommand EditHallCommand { get; protected set; }

    public ICommand EditCommand { get; protected set; }

    #endregion

    #region Constructors

    public WareHouseCreatorPageViewModel(IApp app, IHallService hallService, IRackService rackService) : base(app)
    {
        Page = Models.Page.EApplicationPage.WareHouseCreator;
        _hallService = hallService;
        _rackService = rackService;
        EditHallCommand = new AsyncRelayCommand(() => EditHall(), (o) => CanEdit);
        EditCommand = new RelayCommand((o) => { CanEdit = true; ToSave = true; }, (o) => !CanEdit) ;
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
        if (hall == null)
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
        if (Hall == null || !ToSave)
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

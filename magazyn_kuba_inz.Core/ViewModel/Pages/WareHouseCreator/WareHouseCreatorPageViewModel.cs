using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Models;
using magazyn_kuba_inz.Core.Service;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.WareHouse;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel.Pages;

public class WareHouseCreatorPageViewModel : BasePageViewModel
{
    #region Private fields

    private IHallService _hallService;

    private HallObject _hall;

    private BaseObject _selectedObject;

    #endregion

    #region Public properties

    public HallObject Hall
    {
        get => _hall;
        set { SetProperty(ref _hall, value, nameof(Hall)); }
    }

    public BaseObject SelectedObject
    {
        get => _selectedObject;
        set { SetProperty(ref _selectedObject, value, nameof(SelectedObject)); }
    }

    #endregion

    #region Commands

    public ICommand AddRackCommand { get; protected set; }
    public ICommand RemoveRackCommand { get; protected set; }

    #endregion

    #region Constructors

    public WareHouseCreatorPageViewModel(IApp app, IHallService hallService) : base(app)
    {
        _hallService = hallService;
        AddRackCommand = new RelayCommand((o) => AddRack());
        RemoveRackCommand = new RelayCommand((o) => RemoveRack(Hall.Racks.FirstOrDefault()));
    }

    #endregion

    #region Private methods

    public override void OnPageOpen()
    {
        base.OnPageOpen(); 
        var hall = _hallService.GetAll().FirstOrDefault();
        Hall = _hallService.GetHallObject(hall.ID);
    }

    public override void OnPageClose()
    {
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

    private void AddRack()
    {
        //Hall.Racks.Add(new RackObject(Guid.NewGuid())
        //{
        //    Name = "Rack",
        //    X = 100,
        //    Y = 100,
        //    Color = new SolidColorBrush(Color.FromRgb(255, 0, 0))
        //});
    }

    private void RemoveRack(RackObject rack)
    {
        //if(rack != null)
        //    Hall.Racks.Remove(rack);
    }

    #endregion

    #region Public Methods



    #endregion

    #region Private methods


    #endregion
}

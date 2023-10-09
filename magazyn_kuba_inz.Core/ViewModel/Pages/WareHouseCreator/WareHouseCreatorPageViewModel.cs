using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Models;
using magazyn_kuba_inz.Core.Service;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace magazyn_kuba_inz.Core.ViewModel.Pages;

public class WareHouseCreatorPageViewModel : BasePageViewModel
{
    #region Private fields

    private HallObject _hall;

    private BaseObject _selectedObject;
    
    private double _width;

    private double _height;

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

    public double Width
    {
        get => _width;
        set { SetProperty(ref _width, value, nameof(Width)); }
    }

    public double Height
    {
        get => _height;
        set { SetProperty(ref _height, value, nameof(Height)); }
    }

    #endregion

    #region Commands

    public ICommand AddRackCommand { get; protected set; }
    public ICommand RemoveRackCommand { get; protected set; }

    #endregion

    #region Constructors

    public WareHouseCreatorPageViewModel(IApp app, IHallService hallService) : base(app)
    {
        Width = 1000;
        Height = 1000;
        AddRackCommand = new RelayCommand((o) => AddRack());
        RemoveRackCommand = new RelayCommand((o) => RemoveRack(Hall.Racks.FirstOrDefault()));
        var list = hallService.GetAll();
        var test = hallService.GetHallObject(Guid.Empty);

    }

    #endregion

    #region Private methods

    #endregion

    #region Command Methods

    private void AddRack()
    {
        Hall.Racks.Add(new RackObject(Guid.NewGuid())
        {
            Name = "Rack",
            X = 100,
            Y = 100,
            Color = new SolidColorBrush(Color.FromRgb(255, 0, 0))
        });
    }

    private void RemoveRack(RackObject rack)
    {
        if(rack != null)
            Hall.Racks.Remove(rack);
    }

    #endregion

    #region Public Methods



    #endregion

    #region Private methods


    #endregion
}

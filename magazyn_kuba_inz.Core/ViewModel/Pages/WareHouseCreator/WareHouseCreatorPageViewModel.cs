﻿using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Models;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.WareHouse.Object;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace magazyn_kuba_inz.Core.ViewModel.Pages;

public class WareHouseCreatorPageViewModel : BasePageViewModel
{
    #region Private fields

    private ObservableCollection<RackObject> _racks = new ObservableCollection<RackObject>();

    private ObservableCollection<WayPointObject> _wayPoints = new ObservableCollection<WayPointObject>();

    private RackObject _selectedRack;

    private BaseObject _selectedObject;
    
    private double _width;

    private double _height;

    #endregion

    #region Public properties

    public ObservableCollection<RackObject> Racks
    {
        get => _racks;
        set { SetProperty(ref _racks, value, nameof(Racks)); }
    }

    public ObservableCollection<WayPointObject> WayPoints
    {
        get => _wayPoints;
        set { SetProperty(ref _wayPoints, value, nameof(WayPoints)); }
    }

    public RackObject SelectedRack
    {
        get => _selectedRack;
        set { SetProperty(ref _selectedRack, value, nameof(SelectedRack)); }
    }

    public BaseObject SelectedObject
    {
        get => _selectedObject;
        set { SetProperty(ref _selectedObject, value, nameof(SelectedObject), () => { if (value is RackObject ro) SelectedRack = ro;  }); }
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

    public WareHouseCreatorPageViewModel(IApp app) : base(app)
    {
        Width = 1000;
        Height = 1000;
        AddRackCommand = new RelayCommand((o) => AddRack());
        RemoveRackCommand = new RelayCommand((o) => RemoveRack(Racks.FirstOrDefault()));
        WayPoints = new ObservableCollection<WayPointObject>()
        {
            new WayPointObject()
            {
                X= 250,
                Y = 250
            },
            new WayPointObject()
            {
                X= 500,
                Y = 500
            }
        };

        Racks = new ObservableCollection<RackObject>()
        {
            new RackObject()
            {
                Name = "Rack",
                X = 100,
                Y = 100,
                Color = new SolidColorBrush(Color.FromRgb(255, 0, 0))
            },
            new RackObject()
            {
                Name = "Rack1",
                X = 200,
                Y = 150,
                Color = new SolidColorBrush(Color.FromRgb(255, 0, 0))
            }
        };
    }

    #endregion

    #region Private methods

    #endregion

    #region Command Methods

    private void AddRack()
    {
        Racks.Add(new RackObject()
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
            Racks.Remove(rack);
    }

    #endregion

    #region Public Methods



    #endregion

    #region Private methods


    #endregion
}

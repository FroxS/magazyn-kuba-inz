using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Models;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace magazyn_kuba_inz.Core.ViewModel.Pages;

public class WareHouseCreatorPageViewModel : BasePageViewModel
{
    #region Private fields

    private ObservableCollection<RackObject> _racks;

    private RackObject _selectedRack;

    private double _width;

    private double _height;

    #endregion

    #region Public properties

    public ObservableCollection<RackObject> Racks
    {
        get => _racks;
        set { SetProperty(ref _racks, value, nameof(Racks)); }
    }

    public RackObject SelectedRack
    {
        get => _selectedRack;
        set { SetProperty(ref _selectedRack, value, nameof(SelectedRack)); }
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

    #endregion

    #region Constructors

    public WareHouseCreatorPageViewModel(IApp app) : base(app)
    {
        Width = 1000;
        Height = 1000;

        Racks = new ObservableCollection<RackObject> {
            new RackObject(){
                Position = new Point(100,100),
                Color = new SolidColorBrush(Color.FromRgb(255,0,0))
            },
            new RackObject(){
                Position = new Point(200,100),
                Color = new SolidColorBrush(Color.FromRgb(0,255,0))
            }
        };
        AddRackCommand = new RelayCommand((o) => AddRack());
    }

    #endregion

    #region Private methods

    #endregion

    #region Command Methods

    private void AddRack()
    {
        Racks = new ObservableCollection<RackObject> {
            new RackObject(){
                Position = new Point(100,100),
                Color = new SolidColorBrush(Color.FromRgb(255,0,0))
            },
            new RackObject(){
                Position = new Point(200,100),
                Color = new SolidColorBrush(Color.FromRgb(0,255,0))
            }
        };
    }

    #endregion

    #region Public Methods



    #endregion
}

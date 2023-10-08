using magazyn_kuba_inz.Models.Application;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace magazyn_kuba_inz.Core.Models;

public class RackObject : BaseObject
{
    #region Private fields

    private string _name;

    private Brush _color = new BrushConverter().ConvertFrom("#ff8c00") as SolidColorBrush;

    private double _width = 50;

    private double _height = 50;

    private ObservableCollection<WayPointObject> _wayPoints = new ObservableCollection<WayPointObject>();

    #endregion

    #region Public properties

    public string Name 
    {
        get => _name;
        set { SetProperty(ref _name, value, nameof(Name)); }
    }    

    public Brush Color
    {
        get => _color;
        set { SetProperty(ref _color, value, nameof(Color)); }
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

    public ObservableCollection<WayPointObject> WayPoints
    {
        get => _wayPoints;
        set { SetProperty(ref _wayPoints, value, nameof(WayPoints)); }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public RackObject():base()
    {
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public RackObject(double x, double y) : base(x,y) { }

    /// <summary>
    /// Default constructor
    /// </summary>
    public RackObject(Point point) : base(point) { }


    #endregion

    #region Public methods

    public void SetColor(string hex)
    {
        Color = new BrushConverter().ConvertFrom("#hex") as SolidColorBrush;
    }

    public void AddConnection(ref WayPointObject connection, bool toWay = true)
    {
        if (WayPoints == null)
            WayPoints = new ObservableCollection<WayPointObject>();

        if (!WayPoints.Contains(connection))
            WayPoints.Add(connection);

        RackObject self = this;

        if (toWay)
            connection.AddConnection(ref self, false);

    }

    #endregion
}
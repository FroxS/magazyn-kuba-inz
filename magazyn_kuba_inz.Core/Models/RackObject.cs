using magazyn_kuba_inz.Models.WareHouse;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace magazyn_kuba_inz.Core.Models;

public class RackObject : BaseObject
{
    #region Private fields

    private Guid _id;

    private string _name;

    private Brush _color = new BrushConverter().ConvertFrom("#ff8c00") as SolidColorBrush;

    private double _width = 50;

    private double _height = 50;

    private ObservableCollection<WayPointObject> _wayPoints = new ObservableCollection<WayPointObject>();

    #endregion

    #region Public properties

    public Guid Id
    {
        get => _id;
        private set { SetProperty(ref _id, value, nameof(Id)); }
    }

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
    [JsonConstructor]
    public RackObject(Guid id):base()
    {
        Id = id;
    }



    /// <summary>
    /// Default constructor
    /// </summary>
    public RackObject(Guid id ,double x, double y) : base(x,y) 
    {
        Id = id;
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public RackObject(Guid id, Point point) : base(point) 
    {
        Id = id;
    }

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

    public bool Intersects(RackObject rack)
    {
        double left1 = X - Width / 2;
        double right1 = X + Width / 2;
        double top1 = Y - Height / 2;
        double bottom1 = Y + Height / 2;

        // Obliczamy lewą, prawą, górną i dolną krawędź obiektu other
        double left2 = rack.X - rack.Width / 2;
        double right2 = rack.X + rack.Width / 2;
        double top2 = rack.Y - rack.Height / 2;
        double bottom2 = rack.Y + rack.Height / 2;

        // Sprawdzamy, czy obiekty nachodzą na siebie
        bool horizontalOverlap = left1 <= right2 && right1 >= left2;
        bool verticalOverlap = top1 <= bottom2 && bottom1 >= top2;

        return horizontalOverlap && verticalOverlap;
    }

    #endregion
}
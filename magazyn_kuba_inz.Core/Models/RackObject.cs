using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using Warehouse.Models;

namespace Warehouse.Core.Models;

public class RackObject : Rack, IBaseObject
{
    #region Private fields

    private string _name;

    private Brush _color = new BrushConverter().ConvertFrom("#ff8c00") as SolidColorBrush;

    private ObservableCollection<WayPointObject> _wayPoints = new ObservableCollection<WayPointObject>();

    protected double _x = 0;

    protected double _y = 0;

    protected bool _isSelected = false;

    [JsonIgnore]
    protected ObservableCollection<StorageItem> _items = new ObservableCollection<StorageItem>();

    [JsonIgnore]
    protected bool _canEdit = true;

    #endregion

    #region Public properties

    public override Guid ID
    {
        get => base.ID;
        set { base.ID = value; U(() => ID); }
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
       
    public override double Width
    {
        get => base.Width;
        set { base.Width = value; U(() => Width); }
    }

    public override double Heigth
    {
        get => base.Heigth;
        set { base.Heigth = value; U(() => Heigth); }
    }

    public override int Flors
    {
        get => base.Flors;
        set { base.Flors = value; U(() => Flors); }
    }

    public ObservableCollection<WayPointObject> WayPoints
    {
        get => _wayPoints;
        set { SetProperty(ref _wayPoints, value, nameof(WayPoints)); }
    }

    public Point Position => new Point(X, Y);

    public double X
    {
        get => _x;
        set { SetProperty(ref _x, value); }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set { SetProperty(ref _isSelected, value); }
    }

    [JsonIgnore]
    public bool CanEdit
    {
        get => _canEdit;
        set { SetProperty(ref _canEdit, value); }
    }

    public double Y
    {
        get => _y;
        set { SetProperty(ref _y, value); }
    }

    [JsonIgnore]
    public ObservableCollection<StorageItem> Items
    {
        get => _items;
        set { SetProperty(ref _items, value, nameof(Items)); }
    }

    [JsonIgnore]
    public ICommand OpenRackCommand { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    [JsonConstructor]
    public RackObject(Guid id):base()
    {
        ID = id;
        if (Width == 0)
            Width = 50;
        if (Heigth == 0)
            Heigth = 50;
        OpenRackCommand = new RelayCommand(OpenRack);
    }

    private void OpenRack()
    {
        IApp app = WareHouseApp.App;
        if (app == null)
            return;
        app.Navigation.OpenRack(ID);
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public RackObject(Guid id ,double x, double y):this(id)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public RackObject(Guid id, Point point) : this(id,point.X,point.Y) 
    {
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
        double top1 = Y - Heigth / 2;
        double bottom1 = Y + Heigth / 2;

        // Obliczamy lewą, prawą, górną i dolną krawędź obiektu other
        double left2 = rack.X - rack.Width / 2;
        double right2 = rack.X + rack.Width / 2;
        double top2 = rack.Y - rack.Heigth / 2;
        double bottom2 = rack.Y + rack.Heigth / 2;

        // Sprawdzamy, czy obiekty nachodzą na siebie
        bool horizontalOverlap = left1 <= right2 && right1 >= left2;
        bool verticalOverlap = top1 <= bottom2 && bottom1 >= top2;

        return horizontalOverlap && verticalOverlap;
    }

    public List<Product> GetProducts()
    {
        return StorageItems?.SelectMany(x => x.Items)?.Select(x => x.Product)?.ToList() ?? new List<Product>();
    }

    public bool HasPrduct(Product product)
    {
        return GetProducts().Any(x => x.Equals(product));
    }

    #endregion
}
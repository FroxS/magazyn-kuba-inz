using System.Collections.ObjectModel;
using System.Windows;

namespace Warehouse.Core.Models;

public class WayPointObject : BaseObject
{
    #region Private fields

    protected ObservableCollection<WayPointObject> _connections = new ObservableCollection<WayPointObject>();

    protected ObservableCollection<RackObject> _racks = new ObservableCollection<RackObject>();

    protected bool _isStartPoint = false;

    #endregion

    #region Public properties 

    public ObservableCollection<WayPointObject> Connections
    {
        get => _connections;
        set { SetProperty(ref _connections, value, nameof(_connections)); }
    }

    public ObservableCollection<RackObject> Racks
    {
        get => _racks;
        set { SetProperty(ref _racks, value, nameof(Racks)); }
    }

    public bool IsStartPoint
    {
        get => _isStartPoint;
        set { SetProperty(ref _isStartPoint, value, nameof(IsStartPoint)); }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public WayPointObject()
    {

    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public WayPointObject(double x, double y) : base(x, y) { }

    /// <summary>
    /// Default constructor
    /// </summary>
    public WayPointObject(Point point) : base(point) { }

    #endregion

    #region Private methods

    private double Distance(WayPointObject p1, WayPointObject p2) => Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));

    #endregion

    #region Public methods

    public void AddConnection(ref WayPointObject connection, bool toWay = true) 
    {
        if (Connections == null)
            Connections = new ObservableCollection<WayPointObject>();

        if(!Connections.Contains(connection))
            Connections.Add(connection);

        WayPointObject self = this;

        if (toWay)
            connection.AddConnection(ref self, false);
    }

    public void AddConnection(ref RackObject connection, bool toWay = true)
    {
        if (Racks == null)
            Racks = new ObservableCollection<RackObject>();

        if (!Racks.Contains(connection))
            Racks.Add(connection);

        WayPointObject self = this;

        if (toWay)
            connection.AddConnection(ref self, false);

    }

    public WayPointObject FoundTheNearestPoint(IEnumerable<WayPointObject> points)
    {
        if (points == null || points.Count() == 0)
        {
            return null;
        }
        WayPointObject nearestPoint = null;
        double minDist2 = double.MaxValue;
        foreach (WayPointObject p in points)
        {
            double dist2 = Distance(p, this);
            if (dist2 < minDist2)
            {
                minDist2 = dist2;
                nearestPoint = p;
            }
        }
        return nearestPoint;
    }

    #endregion
}

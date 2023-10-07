using magazyn_kuba_inz.Models.Application;
using System.Windows;

namespace magazyn_kuba_inz.Core.Models;

public class BaseObject : ObservableObject
{
    #region Private fields

    protected double _x = 0;

    protected double _y = 0;

    #endregion

    #region Public properties 

    public Point Position => new Point(Y, Y);

    public double X
    {
        get => _x;
        set { SetProperty(ref _x, value, nameof(X)); }
    }

    public double Y
    {
        get => _y;
        set { SetProperty(ref _y, value, nameof(Y)); }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public BaseObject()
    {
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public BaseObject(double x, double y) : this()
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public BaseObject(Point point) : this(point.X, point.Y) { }

    #endregion
}
using System.Windows;
using Warehouse.Models;

namespace Warehouse.Core.Models;

public class BaseObject : ObservableObject
{
    #region Private fields

    protected double _x = 0;

    protected double _y = 0;

    protected bool _isSelected = false;

    #endregion

    #region Public properties 

    public Point Position => new Point(X, Y);

    public double X
    {
        get => _x;
        set { SetProperty(ref _x, value, nameof(X)); }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set { SetProperty(ref _isSelected, value, nameof(IsSelected)); }
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
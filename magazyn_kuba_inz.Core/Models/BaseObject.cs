﻿using Newtonsoft.Json;
using System.Windows;
using Warehouse.Core.Interface;
using Warehouse.Models;

namespace Warehouse.Core.Models;

public class BaseObject : ObservableObject, IBaseObject
{
    #region Private fields

    protected double _x = 0;

    protected double _y = 0;

    protected bool _isSelected = false;

    [JsonIgnore]
    protected bool _canEdit = true;

    #endregion

    #region Public properties 

    public Point Position => new Point(X, Y);

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

    public bool IsSelected
    {
        get => _isSelected;
        set { SetProperty(ref _isSelected, value, nameof(IsSelected)); }
    }

    [JsonIgnore]
    public bool CanEdit
    {
        get => _canEdit;
        set { SetProperty(ref _canEdit, value); }
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

    #region Public methods

    public double GetDistance(BaseObject toPoint)
    {
        double deltaX = toPoint.X - this.X;
        double deltaY = toPoint.Y - this.Y;
        double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        return distance;
    }

    #endregion
}
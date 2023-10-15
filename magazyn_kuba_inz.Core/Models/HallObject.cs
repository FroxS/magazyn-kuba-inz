﻿using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Warehouse.Models;

namespace Warehouse.Core.Models;

public class HallObject : ObservableObject
{
    #region Private fields

    private Guid _id;

    private string _name;

    private double _width;

    private double _height;

    private ObservableCollection<WayPointObject> _wayPoints = new ObservableCollection<WayPointObject>();

    private ObservableCollection<RackObject> _rackObjects = new ObservableCollection<RackObject>();

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

    public ObservableCollection<RackObject> Racks
    {
        get => _rackObjects;
        set { SetProperty(ref _rackObjects, value, nameof(Racks)); }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    [JsonConstructor]
    public HallObject(Guid id)
    {
        Id = id;
    }

    #endregion

    #region Public methods

    public void AddRacks(params RackObject[] racks)
    {
        foreach (RackObject rack in racks)
        {
            if (Racks.FirstOrDefault(x => x.Id == rack.Id) == null)
                Racks.Add(rack);
        }    
    }

    public void AddPoints(params WayPointObject[] points)
    {
        foreach (WayPointObject point in points)
            if (WayPoints.FirstOrDefault(x => x.Position == point.Position) == null)
                WayPoints.Add(point);
    }

    #endregion
}
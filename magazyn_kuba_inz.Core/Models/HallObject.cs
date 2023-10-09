using magazyn_kuba_inz.Models.Application;
using System.Collections.ObjectModel;

namespace magazyn_kuba_inz.Core.Models;

public class HallObject : ObservableObject
{
    #region Private fields

    private string _name;

    private ObservableCollection<WayPointObject> _wayPoints = new ObservableCollection<WayPointObject>();

    private ObservableCollection<RackObject> _rackObjects = new ObservableCollection<RackObject>();

    #endregion

    #region Public properties

    public string Name 
    {
        get => _name;
        set { SetProperty(ref _name, value, nameof(Name)); }
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
    public HallObject()
    {

    }

    #endregion

    #region Public methods

    #endregion
}
using magazyn_kuba_inz.Core.Models;
using System.Windows;

namespace magazyn_kuba_inz.Models.WareHouse.Object;

public class WayPointObject : BaseObject
{
    #region Public properties


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
}

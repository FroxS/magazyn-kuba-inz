using magazyn_kuba_inz.Core.Models;
using System.Drawing;

namespace magazyn_kuba_inz.Models.WareHouse.Object;

public class WayPointObject
{
    #region Public properties

    public PointF Position { get; set; }

    public RackObject[] RackAssign { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public WayPointObject()
    {

    }

    #endregion
}

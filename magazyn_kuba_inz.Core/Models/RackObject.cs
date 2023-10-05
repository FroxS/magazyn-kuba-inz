using System.Windows;
using System.Windows.Media;

namespace magazyn_kuba_inz.Core.Models;

public class RackObject
{
    #region Public properties

    public Point Position { get; set; } = new Point();

    public Brush Color { get; set; } = new BrushConverter().ConvertFrom("#ff8c00") as SolidColorBrush;

    public double Width { get; set; } = 50;

    public double Height { get; set; } = 50;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public RackObject()
    {

    }

    #endregion

    #region Public methods

    public void SetColor(string hex)
    {
        Color = new BrushConverter().ConvertFrom("#hex") as SolidColorBrush;
    }

    #endregion
}
using Warehouse.Core.Helpers;
using Warehouse.Theme.Conventers;
using System;
using System.Globalization;
using System.Windows.Media;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class ContrastBrushConventer : BaseValueConventer<ContrastBrushConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Brush brush)
            return brush.GetContrastBrush();
        else
            return Brushes.White;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Brush brush)
            return brush.GetContrastBrush();
        else
            return Brushes.White;
    }

}

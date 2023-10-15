using Warehouse.Theme.Conventers;
using System;
using System.Globalization;
using System.Windows.Media;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class BoolBlurEffectConventer : BaseValueConventer<BoolBlurEffectConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter == null)
            return (bool)value ? 5.0 : 0.0;
        else
            return (bool)value ? 0.0 : 5.0;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value as Geometry)?.ToString() ?? "";
    }

}

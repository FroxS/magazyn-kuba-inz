using Warehouse.Theme.Conventers;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class NullVisibleConventer : BaseValueConventer<NullVisibleConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter == null)
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        else
            return value == null ? Visibility.Visible : Visibility.Collapsed;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value as Geometry)?.ToString() ?? "";
    }

}

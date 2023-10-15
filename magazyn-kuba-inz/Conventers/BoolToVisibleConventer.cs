using Warehouse.Core.Models;
using Warehouse.Helper;
using Warehouse.Theme.Conventers;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class BoolToVisibleConventer : BaseValueConventer<BoolToVisibleConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter == null)
            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        else
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value as Geometry)?.ToString() ?? "";
    }

}

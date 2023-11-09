using Warehouse.Theme.Conventers;
using System;
using System.Globalization;
using System.Windows;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class WindowStateToIconConventer : BaseValueConventer<WindowStateToIconConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is WindowState state)
        {
            if (state != WindowState.Maximized)
                return Application.Current.FindResource("Expand");
            else
                return Application.Current.FindResource("Collapse");
        }
        return value;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

}

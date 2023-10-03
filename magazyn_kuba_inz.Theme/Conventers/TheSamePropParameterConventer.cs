using magazyn_kuba_inz.Models.Page;
using System;
using System.Globalization;
using System.Windows;

namespace magazyn_kuba_inz.Theme.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class TheSamePropParameterConventer : BaseValueConventer<TheSamePropParameterConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var val1 = (ApplicationPage)value;
        var val2 = (ApplicationPage)parameter;
        
        return val1 == val2;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

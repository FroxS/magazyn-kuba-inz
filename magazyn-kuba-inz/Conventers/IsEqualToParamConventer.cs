using magazyn_kuba_inz.Theme.Conventers;
using System;
using System.Globalization;
using System.Windows.Media;

namespace magazyn_kuba_inz.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class IsEqualToParamConventer : BaseValueConventer<IsEqualToParamConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.Equals(parameter);
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return parameter;
    }

}

public class IsNotEqualToParamConventer : BaseValueConventer<IsEqualToParamConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !value.Equals(parameter);
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return parameter;
    }

}

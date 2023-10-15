using Warehouse.Theme.Conventers;
using System;
using System.Globalization;
using System.Windows.Media;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class CorrectPointConventer : BaseValueConventer<CorrectPointConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double val, width = 0;
        if(double.TryParse(value.ToString(), out val) && double.TryParse(parameter.ToString(),out width))
        {
            return val - width/2 ;
        }
        return value;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double val, width = 0;
        if (double.TryParse(value.ToString(), out val) && double.TryParse(parameter.ToString(), out width))
        {
            return val + width / 2;
        }
        return value;
    }

}

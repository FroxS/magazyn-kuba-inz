using Warehouse.Theme.Conventers;
using System;
using System.Globalization;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class SelectValueByFirstBoolConventer : BaseMultiValueConventer<SelectValueByFirstBoolConventer>
{
    public override object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value.Length == 3 && value[0] is bool flag)
        {
            return flag ? value[1] : value[2];
        }
        return value;
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

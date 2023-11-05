using Warehouse.Theme.Conventers;
using System;
using System.Globalization;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class MulitBindingParameterConventer : BaseMultiValueConventer<MulitBindingParameterConventer>
{
    public override object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.Clone() ;
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

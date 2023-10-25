using Warehouse.Core.Helpers;
using Warehouse.Theme.Conventers;
using System;
using System.Globalization;
using Warehouse.Models;
using Warehouse.Core.Models;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class RackNameConventer : BaseValueConventer<RackNameConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Rack rack)
            return rack.GetName();
        return value;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}

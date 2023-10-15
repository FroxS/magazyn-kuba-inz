using Warehouse.Theme.Conventers;
using System;
using System.Globalization;
using System.Windows;
using Warehouse.Models.Enums;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class MessageTypeToColorConventer : BaseValueConventer<MessageTypeToColorConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        string colorresourceName = "WaringColorBrush";
        if(value is EMessageType mt)
        {
            switch (mt)
            {
                case EMessageType.Ok:
                    colorresourceName = "GoodColorBrush";
                    break;
                case EMessageType.Warning:
                    colorresourceName = "WaringColorBrush";
                    break;
                case EMessageType.Error:
                    colorresourceName = "WrongColorBrush";
                    break;
            }
        }

        return Application.Current.Resources[colorresourceName] as System.Windows.Media.SolidColorBrush;
        
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

}

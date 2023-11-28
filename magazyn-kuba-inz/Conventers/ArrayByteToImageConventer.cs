using Warehouse.Helper;
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
public class ArrayByteToImageConventer : BaseValueConventer<ArrayByteToImageConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        ImageSource? image = null;

        if (value is ImageSource)
            return value;

        if(value is byte[] byt)
        {
            image = byt.GetImage();
        }

        if(image == null)
        {
            var guest = Application.Current.Resources["User"];

            if (guest is Style style)
            {
                foreach (Setter t in style.Setters)
                {
                    if (t.Property.Name == "Data")
                    {
                        if(t.Value is Geometry g)
                        {
                            image = g.GetImage(); 
                        }
                    }
                }
            }
        }
        return image;
    }


    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value as Geometry)?.ToString() ?? "";
    }

    public Geometry GetGeometry(string name)
    {
        try
        {
            var obj = Application.Current.Resources[name];
            if (obj is Style style)
            {
                foreach (Setter t in style.Setters)
                {
                    if (t.Property.Name == "Data")
                        return (t.Value as Geometry) ?? Geometry.Empty;
                }
            }
            return Geometry.Empty;
        }
        catch { return Geometry.Empty; }
        
    }
}

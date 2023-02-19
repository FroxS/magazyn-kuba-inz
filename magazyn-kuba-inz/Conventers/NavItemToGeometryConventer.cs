using magazyn_kuba_inz.Core.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace magazyn_kuba_inz.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class NavItemToGeometryConventer : BaseValueConventer<NavItemToGeometryConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is NavItemType type)
        {

            switch (type)
            {
                case NavItemType.Dashboard:
                    return GetGeometry("Home");
                case NavItemType.Settings:
                    return GetGeometry("Settings");

            }
        }
        return Geometry.Empty;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value as Geometry)?.ToString() ?? "";
    }

    public Geometry GetGeometry(string name)
    {
        try
        {
            var obj = Application.Current.TryFindResource(name);
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

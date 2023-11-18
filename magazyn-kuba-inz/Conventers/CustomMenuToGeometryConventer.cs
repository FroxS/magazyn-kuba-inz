using Warehouse.Theme.Conventers;
using System;
using System.Globalization;
using System.Windows;
using Warehouse.Models.Page;
using Warehouse.ViewModels.Navigation;
using System.Windows.Media;

namespace Warehouse.Conventers;

/// <summary>
/// Base value conventer that allows direct XMAL usage
/// </summary>
/// <typeparam name="T">The type of this value conventer </typeparam>
public class CustomMenuToGeometryConventer : BaseValueConventer<CustomMenuToGeometryConventer>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is CustomMenuItem menu)
        {
            EApplicationPage page = menu.PageType;

            if (!string.IsNullOrEmpty(menu?.ResourceIconName))
            {
                var iconfromname = Application.Current.TryFindResource(menu.ResourceIconName);
                if (iconfromname != null && iconfromname is Geometry)
                    return iconfromname;
            }

            switch (page) 
            {
                case EApplicationPage.DashBoard:
                    return Application.Current.TryFindResource("HomeGeometry");
                case EApplicationPage.Order:
                    return Application.Current.TryFindResource("Order");
                case EApplicationPage.WareHouseItems:
                    return Application.Current.TryFindResource("WarehouseItems");
                case EApplicationPage.Products:
                    return Application.Current.TryFindResource("ProductsGeometry");
                case EApplicationPage.Suppliers:
                    return Application.Current.TryFindResource("SupplierHeometry");
                case EApplicationPage.ProductGroups:
                    return Application.Current.TryFindResource("CategoryGeometry");
                case EApplicationPage.ProductStatuses:
                    return Application.Current.TryFindResource("ProductStatus");
                case EApplicationPage.ItemStates:
                    return Application.Current.TryFindResource("ItemState");
                case EApplicationPage.StorageUnits:
                    return Application.Current.TryFindResource("Box");
                case EApplicationPage.Racks:
                    return Application.Current.TryFindResource("Rack");
                case EApplicationPage.WareHouseCreator:
                    return Application.Current.TryFindResource("Creator");
                case EApplicationPage.Settings:
                    return Application.Current.TryFindResource("SettingsGeometry");
            }
        }
        return value;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

}

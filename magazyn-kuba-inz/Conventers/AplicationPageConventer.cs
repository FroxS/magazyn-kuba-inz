using Warehouse.View.Pages;
using System;
using System.Diagnostics;
using Warehouse.Models.Page;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;

namespace Warehouse.Conventers;

public static class AplicationPageConventer
{
    /// <summary>
    /// Takes a <see cref="EApplicationPage"/> and the viem model if any and creates the desired page
    /// </summary>
    /// <param name="page"></param>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public static IBasePage ToBasePage(this IBasePageViewModel pagevm, IServiceProvider services) 
    {
        switch (pagevm.Page)
        {
            case EApplicationPage.DashBoard:
                return new DashBoardPage(pagevm);
            case EApplicationPage.Products:
                return new ProductsPage(pagevm);
            case EApplicationPage.Suppliers:
                return new SuppliersPage(pagevm);
            case EApplicationPage.ProductGroups:
                return new ProductGroupPage(pagevm);
            case EApplicationPage.Settings:
                return new SettingsPage(pagevm);
            case EApplicationPage.ProductStatuses:
                return new ProductStatusesPage(pagevm);
            case EApplicationPage.ItemStates:
                return new ItemStatesPage(pagevm);
            case EApplicationPage.WareHouseItems:
                return new WareHouseItemsPage(pagevm);
            case EApplicationPage.StorageUnits:
                return new StorageUnitsPage(pagevm);
            case EApplicationPage.WareHouseCreator:
                return new WareHouseCreatorPage(pagevm);
            case EApplicationPage.Racks:
                return new RacksPage(pagevm);
            case EApplicationPage.Order:
                return new OrdersPage(pagevm);
            case EApplicationPage.WayToOrder:
                return new WayToOrderPage(pagevm);
            case EApplicationPage.EditAddOrder:
                return new OrderEditAddPage(pagevm);
            default:
                Debugger.Break();
                return null;
        }
    }
}

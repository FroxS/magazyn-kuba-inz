using Warehouse.View.Pages;
using System;
using System.Diagnostics;
using Warehouse.Models.Page;
using Warehouse.Service.Interface;
using Warehouse.ViewModel.Service;
using Warehouse.ViewModel.Pages;
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
                return new OrderPage(pagevm);
            case EApplicationPage.WayToOrder:
                return new WayToOrderPage(pagevm);
            default:
                Debugger.Break();
                return null;
        }
    }

    /// <summary>
    /// Takes a <see cref="BasePageViewModel"/>
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public static EApplicationPage GetPageFromViewModel(this BasePageViewModel vm)
    {

        if (vm is DashBoardPageViewModel)
            return EApplicationPage.DashBoard;
        if (vm is ProductsPageViewModel)
            return EApplicationPage.Products;
        if (vm is SuppliersPageViewModel)
            return EApplicationPage.Suppliers;
        if (vm is ProductStatusesPageViewModel)
            return EApplicationPage.ProductGroups;
        if (vm is SettingsPageViewModel)
            return EApplicationPage.Settings;
        if (vm is ProductStatusesPageViewModel)
            return EApplicationPage.ProductStatuses;
        if (vm is ItemStatesPageViewModel)
            return EApplicationPage.ItemStates;
        if (vm is WareHousePageViewModel)
            return EApplicationPage.WareHouseItems;
        if (vm is StorageUnitsPageViewModel)
            return EApplicationPage.StorageUnits;
        if (vm is RacksPageViewModel)
            return EApplicationPage.Racks;

        Debugger.Break();
        return default(EApplicationPage);
    }
}

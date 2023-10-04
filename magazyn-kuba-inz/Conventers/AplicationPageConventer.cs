using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.Page;
using magazyn_kuba_inz.View.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace magazyn_kuba_inz.Conventers;

public static class AplicationPageConventer
{
    /// <summary>
    /// Takes a <see cref="EApplicationPage"/> and the viem model if any and creates the desired page
    /// </summary>
    /// <param name="page"></param>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public static IBasePage ToBasePage(this EApplicationPage page, IServiceProvider services) 
    {
        switch (page)
        {
            case EApplicationPage.DashBoard:
                return services.GetRequiredService<DashBoardPage>();
            case EApplicationPage.Products:
                return services.GetRequiredService<ProductsPage>();
            case EApplicationPage.Suppliers:
                return services.GetRequiredService<SuppliersPage>();
            case EApplicationPage.ProductGroups:
                return services.GetRequiredService<ProductGroupPage>();
            case EApplicationPage.Settings:
                return services.GetRequiredService<SettingsPage>();
            case EApplicationPage.ProductStatuses:
                return services.GetRequiredService<ProductStatusesPage>();
            case EApplicationPage.ItemStates:
                return services.GetRequiredService<ItemStatesPage>();
            case EApplicationPage.WareHouseItems:
                return services.GetRequiredService<WareHouseItemsPage>();
            case EApplicationPage.StorageUnits:
                return services.GetRequiredService<StorageUnitsPage>();
            case EApplicationPage.WareHouseCreator:
                return services.GetRequiredService<WareHouseCreatorPage>();
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
        if (vm is WareHouseItemsPageViewModel)
            return EApplicationPage.WareHouseItems;
        if (vm is StorageUnitsPageViewModel)
            return EApplicationPage.StorageUnits;

        Debugger.Break();
        return default(EApplicationPage);
    }
}

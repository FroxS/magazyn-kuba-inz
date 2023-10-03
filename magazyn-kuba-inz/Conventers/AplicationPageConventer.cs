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
    /// Takes a <see cref="ApplicationPage"/> and the viem model if any and creates the desired page
    /// </summary>
    /// <param name="page"></param>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public static IBasePage ToBasePage(this ApplicationPage page, IServiceProvider services) 
    {
        switch (page)
        {
            case ApplicationPage.DashBoard:
                return services.GetRequiredService<DashBoardPage>();
            case ApplicationPage.Products:
                return services.GetRequiredService<ProductsPage>();
            case ApplicationPage.Suppliers:
                return services.GetRequiredService<SuppliersPage>();
            case ApplicationPage.ProductGroups:
                return services.GetRequiredService<ProductGroupPage>();
            case ApplicationPage.Settings:
                return services.GetRequiredService<SettingsPage>();
            case ApplicationPage.ProductStatuses:
                return services.GetRequiredService<ProductStatusesPage>();
            case ApplicationPage.ItemStates:
                return services.GetRequiredService<ItemStatesPage>();
            case ApplicationPage.WareHouseItems:
                return services.GetRequiredService<WareHouseItemsPage>();

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
    public static ApplicationPage GetPageFromViewModel(this BasePageViewModel vm)
    {

        if (vm is DashBoardPageViewModel)
            return ApplicationPage.DashBoard;
        if (vm is ProductsPageViewModel)
            return ApplicationPage.Products;
        if (vm is SuppliersPageViewModel)
            return ApplicationPage.Suppliers;
        if (vm is ProductStatusesPageViewModel)
            return ApplicationPage.ProductGroups;
        if (vm is SettingsPageViewModel)
            return ApplicationPage.Settings;
        if (vm is ProductStatusesPageViewModel)
            return ApplicationPage.ProductStatuses;
        if (vm is ItemStatesPageViewModel)
            return ApplicationPage.ItemStates;
        if (vm is WareHouseItemsPageViewModel)
            return ApplicationPage.WareHouseItems;

        Debugger.Break();
        return default(ApplicationPage);
    }
}

using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Models.Page;
using magazyn_kuba_inz.Models.WareHouse;
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

            default:
                Debugger.Break();
                return null;
        }
    }
}

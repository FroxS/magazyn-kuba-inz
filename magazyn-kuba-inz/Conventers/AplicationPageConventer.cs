using magazyn_kuba_inz.Core.Service.Interface;
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

            default:
                Debugger.Break();
                return null;
        }
    }
}

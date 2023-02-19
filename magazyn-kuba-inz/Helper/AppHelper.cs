using magazyn_kuba_inz.Core.Service.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace magazyn_kuba_inz.Helper;

/// <summary>
/// Locate viem models from the IoC gor use in binding in Xaml fiels
/// </summary>
public class AppHelper
{
    #region Public properties
    /// <summary>
    /// Singleton instance fo the lacator
    /// </summary>
    public static AppHelper Instance { get; private set; } = new AppHelper();

    /// <summary>
    /// The aplication view model
    /// </summary>
    public static INavigation Navigation => App.AppHost.Services.GetRequiredService<INavigation>();

    /// <summary>
    /// The aplication view model
    /// </summary>
    public static IApp Application => App.AppHost.Services.GetRequiredService<IApp>();

    #endregion
}


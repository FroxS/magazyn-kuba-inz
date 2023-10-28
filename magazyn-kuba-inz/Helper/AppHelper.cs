using Microsoft.Extensions.DependencyInjection;
using Warehouse.Core.Interface;
using Warehouse.Service;

namespace Warehouse.Helper;

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

    /// <summary>
    /// The Inner dialog service
    /// </summary>
    public static IInnerDialogService InnerDialog => App.AppHost.Services.GetRequiredService<IInnerDialogService>();

    /// <summary>
    /// Message service
    /// </summary>
    public static IMessageService MessageService => App.AppHost.Services.GetRequiredService<IMessageService>();

    #endregion
}


using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Diagnostics;
using Warehouse.Core.Interface;
using Warehouse.Models.Page;
using Warehouse.Service.Interface;

namespace Warehouse.Core.Helpers;

public static class AplicationHelper
{
    #region Public static properties

    public static string GetAplicationPath() => AppDomain.CurrentDomain.BaseDirectory;

    #endregion

}
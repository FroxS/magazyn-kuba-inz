namespace Warehouse.Core.Helpers;

public static class AplicationHelper
{

    #region Public static properties

    public static string GetAplicationPath() => AppDomain.CurrentDomain.BaseDirectory;

    #endregion

}
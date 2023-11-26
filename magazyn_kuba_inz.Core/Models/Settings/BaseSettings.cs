using Warehouse.Models;

namespace Warehouse.Core.Models.Settings;

public abstract class BaseSettings : ObservableObject
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public BaseSettings()
    {

    }

    #endregion

    #region Helpers

    protected string GetJson()
    {
        string json = string.Empty;
        json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
        return json;
    }

    protected string GetApplicationPath() => AppDomain.CurrentDomain.BaseDirectory;

    #endregion

    #region Required metgods

    public abstract void Save();

    #endregion
}
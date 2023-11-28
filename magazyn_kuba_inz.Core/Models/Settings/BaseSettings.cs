using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Reflection;
using Warehouse.Core.Helpers;
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

    protected T? GetData<T>(string json) where T : BaseSettings
    {
        T? obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        return obj;
    }

    protected string GetApplicationPath() => AppDomain.CurrentDomain.BaseDirectory;

    protected void CopyProperties<T>(T source) where T : BaseSettings
    {
        PropertyInfo[] properties = typeof(T).GetProperties();
        foreach (PropertyInfo property in properties)
        {
            object value = property.GetValue(source);

            property.SetValue(this, value);
        }
    }

    #endregion

    #region Required metgods

    public abstract void Save();

    public abstract void Load();

    public virtual string GetJson()
    {
        string json = string.Empty;
        json = JsonConvert.SerializeObject(this);
        return json;
    }

    #endregion
}

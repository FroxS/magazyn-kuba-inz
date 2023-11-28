using System.Text.Json.Serialization;
using Warehouse.Core.Interface;

namespace Warehouse.Core.Models.Settings;

public class GlobalSettings : BaseSettings
{
    #region Private properties

    private IAppSettingsService _service;

    private int _weightDiscrepancy;
    

    #endregion

    #region Public properties

    public int WeightDiscrepancy
    {
        get => _weightDiscrepancy;
        set { SetProperty(ref _weightDiscrepancy, value); }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public GlobalSettings(IAppSettingsService service)
    {
        _service = service;
    }

    [JsonConstructor]
    public GlobalSettings()
    {
        
    }

    public GlobalSettings (IAppSettingsService service, string? json): this(service)
    {
        if (!string.IsNullOrEmpty(json))
        {
            GlobalSettings? settings = GetData<GlobalSettings>(json);
            if (settings != null)
                CopyProperties(settings);
        }
    }

    #endregion

    #region Saving

    public override void Save()
    {
        _service.SaveSettings(this);
    }

    public override void Load()
    {
        GlobalSettings? obj = _service.GetSettings();
        if (obj == null)
            return;

        CopyProperties(obj);
    }

    #endregion
}
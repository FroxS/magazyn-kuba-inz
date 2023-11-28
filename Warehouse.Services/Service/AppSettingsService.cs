using Warehouse.Repository.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Warehouse.Core.Models.Settings;

namespace Warehouse.Service;

internal class AppSettingsService : IAppSettingsService
{
    #region Private fields

    private readonly IAppSettingsRepository _repo;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public AppSettingsService( IAppSettingsRepository repo )
    {
        _repo = repo;
    }

    #endregion

    #region Public methods

    public GlobalSettings GetSettings()
    {
        AppSettings? settingsDB = _repo.GetAll().FirstOrDefault();
        if (settingsDB == null )
            return null;

        GlobalSettings gs = new GlobalSettings(this, settingsDB.Data);
        return gs;
    }

    public void SaveSettings(GlobalSettings settings)
    {
        string json = settings.GetJson();
        AppSettings? settingsDB = _repo.GetAll().FirstOrDefault();
        if(settingsDB == null)
        {
            _repo.Add(new AppSettings()
            {
                ID = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Data = json,
                Lp = 0,
                Modified = DateTime.Now,
            });
            _repo.Save();
        }
        else
        {
            settingsDB.Data = json;
            _repo.Update(settingsDB);
            _repo.Save();
        }
    }

    #endregion
}

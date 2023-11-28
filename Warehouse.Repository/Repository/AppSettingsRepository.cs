using Warehouse.Repository.Interfaces;
using Warehouse.EF;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Interface;

namespace Warehouse.Repository;

internal class AppSettingsRepository : BaseRepository<AppSettings,WarehouseDbContext>, IAppSettingsRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public AppSettingsRepository(IApp app) : base(app)
    {

    }

    #endregion

}
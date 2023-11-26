using Warehouse.Core.Interface;
using Warehouse.EF;

namespace Warehouse.Core.Models.Settings;

public class GlobalSettings : BaseSettings
{
    #region Private properties

    private IApp _app;
    private WarehouseDbContext _database => _app.Database as WarehouseDbContext;

    public int _weightDiscrepancy;
    

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
    public GlobalSettings(IApp app)
    {
        _app = app;
    }

    #endregion

    #region Saving

    public override void Save()
    {
        
    }

    #endregion
}
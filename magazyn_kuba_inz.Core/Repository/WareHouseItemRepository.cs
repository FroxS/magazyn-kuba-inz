using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.EF;
using magazyn_kuba_inz.Models.WareHouse;

namespace magazyn_kuba_inz.Core.Repository;

public class WareHouseItemRepository : BaseRepository<WareHouseItem, WarehouseDbContext>, IWareHouseItemRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public WareHouseItemRepository(WarehouseDbContext context) : base(context)
    {

    }

    #endregion

    #region public methods


    #endregion
}
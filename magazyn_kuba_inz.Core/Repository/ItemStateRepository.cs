using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.EF;
using magazyn_kuba_inz.Models.WareHouse;

namespace magazyn_kuba_inz.Core.Repository;

public class ItemStateRepository : BaseRepository<ItemState, WarehouseDbContext>, IItemStateRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public ItemStateRepository(WarehouseDbContext context) : base(context)
    {

    }

    #endregion

    #region public methods


    #endregion
}
using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.EF;
using magazyn_kuba_inz.Models.WareHouse;

namespace magazyn_kuba_inz.Core.Repository;

public class ProductStatusRepository : BaseRepository<ProductStatus, WarehouseDbContext>, IProductStatusRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public ProductStatusRepository(WarehouseDbContext context) : base(context)
    {

    }

    #endregion

    #region public methods


    #endregion
}
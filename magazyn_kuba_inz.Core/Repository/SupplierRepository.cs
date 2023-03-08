using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.EF;
using magazyn_kuba_inz.Models.WareHouse;
namespace magazyn_kuba_inz.Core.Repository;

public class SupplierRepository : BaseRepository<Supplier,WarehouseDbContext>, ISupplierRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public SupplierRepository(WarehouseDbContext context) : base(context)
    {

    }

    #endregion

    #region public methods


    #endregion
}
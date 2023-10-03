using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.EF;
using magazyn_kuba_inz.Models.WareHouse;
using Microsoft.EntityFrameworkCore;

namespace magazyn_kuba_inz.Core.Repository;

public class ProductStatusRepository : BaseRepository<ProductStatus, WarehouseDbContext>, IProductStatusRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public ProductStatusRepository(IDbContextFactory<WarehouseDbContext> factory) : base(factory)
    {

    }

    #endregion

    #region Public methods

    public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var found = (await _context.ProductStatus.Include(x => x.Products).FirstOrDefaultAsync(x => x.ID == id, cancellationToken));
        if (found == null)
            throw new ArgumentException($"Produt status not found");

        if (found.Products.Count > 0)
        {
            throw new ArgumentException($"Status {found.Name} has assign {found.Products.Count} products");
        }
        _context.Remove(found);
    }

    #endregion
}
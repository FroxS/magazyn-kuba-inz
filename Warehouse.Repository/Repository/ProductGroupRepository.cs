using Warehouse.Repository.Interfaces;
using Warehouse.EF;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Repository;

internal class ProductGroupRepository : BaseRepository<ProductGroup, WarehouseDbContext>, IProductGroupRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public ProductGroupRepository(IDbContextFactory<WarehouseDbContext> factory) : base(factory)
    {

    }

    #endregion

    #region public methods

    public async Task<ProductGroup?> GetProductGroupWithProducts(Guid id, CancellationToken cancellationToken = default(CancellationToken))
    {
        return (await _context.ProductGroups.Include(x => x.Products).FirstOrDefaultAsync(x => x.ID == id, cancellationToken));
    }

    public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var found = (await _context.ProductGroups.Include(x => x.Products).FirstOrDefaultAsync(x => x.ID == id, cancellationToken));
        if (found == null)
            throw new ArgumentException($"Product group not found");

        if (found.Products.Count > 0)
        {
            throw new ArgumentException($"Status {found.Name} has assign {found.Products.Count} products");
        }
        _context.Remove(found);
    }

    #endregion
}
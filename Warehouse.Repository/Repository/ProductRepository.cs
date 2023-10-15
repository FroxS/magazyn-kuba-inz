using Warehouse.Repository.Interfaces;
using Warehouse.EF;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Repository;

internal class ProductRepository : BaseRepository<Product, WarehouseDbContext>, IProductRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public ProductRepository(IDbContextFactory<WarehouseDbContext> factory) : base(factory)
    {

    }

    #endregion

    #region Private methods

    private IQueryable<Product> Get()
    {
        return _context.Products
            .Include(x => x.Supplier)
            .Include(x => x.Status)
            .Include(x => x.Group)
            .Include(x => x.Images);
    }

    #endregion

    #region Public methods


    public async override Task<List<Product>> GetAllAsync(bool sortbylp = true, CancellationToken cancellationToken = default)
    {
        return await Get().OrderBy(x => x.Lp).ToListAsync();
    }

    public override List<Product> GetAll(bool sortbylp = true)
    {
        return Get().OrderBy(x => x.Lp).ToList();
    }

    //public async override Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    //{
    //    return await _context.Products.FindAsync(id, cancellationToken);
    //}
    public async override Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Get().FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
    }

    #endregion
}
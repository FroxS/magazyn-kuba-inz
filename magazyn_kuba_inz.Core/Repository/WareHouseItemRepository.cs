using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.EF;
using magazyn_kuba_inz.Models.WareHouse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace magazyn_kuba_inz.Core.Repository;

public class WareHouseItemRepository : BaseRepository<WareHouseItem, WarehouseDbContext>, IWareHouseItemRepository
{
    #region Properties

    private IQueryable<WareHouseItem> _items => _context.WareHouseItems.Include(x => x.Product).Include(x => x.State);

    #endregion

    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public WareHouseItemRepository(IDbContextFactory<WarehouseDbContext> factory) : base(factory)
    {

    }

    #endregion

    #region Public methods

    public override async Task<WareHouseItem> AddAsync(WareHouseItem entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentException("Entity is null");

        if (entity.ID_Product == Guid.Empty)
            throw new ArgumentException("Not assing product");

        if (entity.ID_State == Guid.Empty)
            throw new ArgumentException("Not assing state");
        return (await _context.WareHouseItems.AddAsync(entity, cancellationToken)).Entity;
    }

    public async Task<WareHouseItem?> GetItemAsync(Guid productId, Guid stateId, CancellationToken cancellationToken = default)
    {
        return await _items.FirstOrDefaultAsync(x => x.ID_Product == productId && x.ID_State == stateId, cancellationToken);
    }

    public async Task<List<WareHouseItem>> GetItemAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _items.Where(x => x.ID_Product == productId).ToListAsync(cancellationToken);
    }

    public async Task<List<Product?>> GetProdictsWidthStateAsync(Guid statusId, CancellationToken cancellationToken = default)
    {
        return await _items.Where(x => x.ID_State == statusId).Select(x => x.Product).ToListAsync(cancellationToken);
    }

    public override async Task<List<WareHouseItem>> GetAllAsync(bool sortbylp = true, CancellationToken cancellationToken = default)
    {
        IQueryable<WareHouseItem> items = _items;
        if (sortbylp)
            items = items.OrderBy(x => x.Lp);
        return await items.ToListAsync(cancellationToken);
    }

    public override async Task<WareHouseItem> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _items.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
    }

    #endregion
}
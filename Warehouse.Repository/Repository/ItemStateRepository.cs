using Warehouse.Repository.Interfaces;
using Warehouse.EF;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using Warehouse.Models.Enums;
using Warehouse.Core.Interface;

namespace Warehouse.Repository;

internal class ItemStateRepository : BaseRepository<ItemState, WarehouseDbContext>, IItemStateRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public ItemStateRepository(IApp app) : base(app)
    {

    }

    #endregion

    #region Public methods

    public async override Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var found = (await _context.ItemStates.Include(x => x.Items).FirstOrDefaultAsync(x => x.ID == id, cancellationToken));
        if (found == null)
            throw new ArgumentException($"Item state not found");

        if (found.Items.Count > 0)
        {
            throw new ArgumentException($"Item {found.Name} has assign {found.Items.Count} products");
        }
        _context.Remove(found);
    }

    public override List<ItemState> GetAll(bool sortbylp = true)
    {
        return base.GetAll(sortbylp).OrderBy(x => x.State).ToList();
    }

    public override List<ItemState> GetAll(Func<IQueryable<ItemState>, IQueryable<ItemState>> include, bool sortbylp = true)
    {
        return base.GetAll(include,sortbylp).OrderBy(x => x.State).ToList();
    }

    public ItemState GetByState(EState state)
    {
        return base.GetAll().FirstOrDefault(x => x.State == state);
    }

    #endregion
}
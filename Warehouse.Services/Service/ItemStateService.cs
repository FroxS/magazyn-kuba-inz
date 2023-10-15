using Warehouse.Repository.Interfaces;
using Warehouse.Service.Interface;
using Warehouse.Models;

namespace Warehouse.Service;

public class ItemStateService : BaseServiceWithRepository<IItemStateRepository, ItemState>, IItemStateService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public ItemStateService(IItemStateRepository repository):base(repository)
    {
        
    }

    #endregion

    #region Public Method
    public async Task<ItemState> AddAsync(ItemState item)
    {
        if (item == null)
            throw new ArgumentException("Product group is null");

        await _repozitory.AddAsync(item);
        return item;
    }

    #endregion
}
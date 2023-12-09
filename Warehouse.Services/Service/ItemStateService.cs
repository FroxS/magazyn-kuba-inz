using Warehouse.Repository.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Warehouse.Models.Enums;

namespace Warehouse.Service;

internal class ItemStateService : BaseServiceWithRepository<IItemStateRepository, ItemState>, IItemStateService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public ItemStateService(IItemStateRepository repozitory, IApp app) : base(repozitory, app)
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


    public ItemState GetByState(EState state)
    {
        return _repozitory.GetByState(state);
    }

    #endregion
}
using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.Models.WareHouse;

namespace magazyn_kuba_inz.Core.Service;

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
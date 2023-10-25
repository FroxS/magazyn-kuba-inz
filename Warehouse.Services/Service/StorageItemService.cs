using Warehouse.Repository.Interfaces;
using Warehouse.Service.Interface;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Service;

internal class StorageItemService : BaseServiceWithRepository<IStorageItemRepository,StorageItem>, IStorageItemService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public StorageItemService(IStorageItemRepository ripository) :base(ripository)
    {
    }

    #endregion

    #region Public Method

    public List<StorageItem>? GetItemsByPackage(Guid id)
    {
        return _repozitory.GetAll(x => x.Include(i => i.Item).ThenInclude(i => i.Product)).Where(x => x.ID_Package == id).ToList();
    }

    #endregion
}
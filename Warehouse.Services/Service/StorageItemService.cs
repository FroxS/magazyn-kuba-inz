using Warehouse.Repository.Interfaces;
using Warehouse.Service.Interface;
using Warehouse.Models;

namespace Warehouse.Service;

public class StorageItemService : BaseServiceWithRepository<IStorageItemRepository,StorageItem>, IStorageItemService
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

    #endregion
}
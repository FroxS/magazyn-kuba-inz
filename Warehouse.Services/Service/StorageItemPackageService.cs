using Warehouse.Repository.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Service;

internal class StorageItemPackageService : BaseServiceWithRepository<IStorageItemPackageRepository, StorageItemPackage>, IStorageItemPackageService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public StorageItemPackageService(IStorageItemPackageRepository ripository) :base(ripository)
    {
    }

    #endregion

    #region Public Method

    public List<StorageItem>? GetItemsByID(Guid id)
    {
        return _repozitory.GetById(x => x.Include(i => i.Items).ThenInclude(i => i.Product),id)?.Items;
    }

    #endregion
}
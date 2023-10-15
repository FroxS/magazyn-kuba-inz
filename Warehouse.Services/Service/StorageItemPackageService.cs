using Warehouse.Repository.Interfaces;
using Warehouse.Service.Interface;
using Warehouse.Models;

namespace Warehouse.Service;

public class StorageItemPackageService : BaseServiceWithRepository<IStorageItemPackageRepository, StorageItemPackage>, IStorageItemPackageService
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

    #endregion
}
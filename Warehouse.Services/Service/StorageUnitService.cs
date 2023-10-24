using Warehouse.Repository.Interfaces;
using Warehouse.Service.Interface;
using Warehouse.Models;

namespace Warehouse.Service;

internal class StorageUnitService : BaseServiceWithRepository<IStorageUnitRepository, StorageUnit>, IStorageUnitService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public StorageUnitService(IStorageUnitRepository ripository) :base(ripository)
    {
    }

    #endregion

    #region Public Method

    #endregion
}
using Warehouse.Repository.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.Models;

namespace Warehouse.Service;

internal class StorageUnitService : BaseServiceWithRepository<IStorageUnitRepository, StorageUnit>, IStorageUnitService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public StorageUnitService(IStorageUnitRepository repozitory, IApp app) : base(repozitory, app)
    {
    }

    #endregion

    #region Public Method

    #endregion
}
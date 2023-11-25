using Warehouse.Repository.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.Models;

namespace Warehouse.Service;

internal class ImageService : BaseServiceWithRepository<IImageRepository,WareHouseImage>, IImageService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public ImageService(IImageRepository repozitory, IApp app) : base(repozitory, app)
    {
    }

    #endregion

    #region Public Method

    #endregion
}
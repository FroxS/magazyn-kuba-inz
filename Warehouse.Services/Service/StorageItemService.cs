﻿using Warehouse.Repository.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Service;

internal class StorageItemService : BaseServiceWithRepository<IStorageItemRepository,StorageItem>, IStorageItemService
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public StorageItemService(IStorageItemRepository repozitory, IApp app) : base(repozitory, app)
    {
    }

    #endregion

    #region Public Method

    public List<StorageItem>? GetItemsByPackage(Guid id)
    {
        return _repozitory.GetAll(x => x.Include(i => i.Product)).Where(x => x.ID_Package == id).ToList();
    }

    #endregion
}
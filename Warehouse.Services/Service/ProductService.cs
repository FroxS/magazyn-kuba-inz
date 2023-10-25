﻿using Warehouse.Repository.Interfaces;
using Warehouse.Service.Interface;
using Warehouse.Models;

namespace Warehouse.Service;

internal class ProductService : BaseServiceWithRepository<IProductRepository,Product>, IProductService
{
    #region Private fields

    private readonly ISupplierService _supplierService;
    private readonly IProductGroupService _productgroupService;
    private readonly IProductStatusService _productStatusService;
    private readonly IImageService _imageService;
    private readonly IWareHouseItemService _wareHouseItemService;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public ProductService(
        IProductRepository repozitory,
        ISupplierService supplierService,
        IProductGroupService productgroupService,
        IProductStatusService productStatusService,
        IImageService imageService,
        IWareHouseItemService wareHouseItemService
        ) 
        : base(repozitory)
    {
        _supplierService = supplierService;
        _productgroupService = productgroupService;
        _productStatusService = productStatusService;
        _imageService = imageService;
        _wareHouseItemService = wareHouseItemService;
    }

    #endregion

    #region Public Method

    public async Task<Product> AddAsync(Product item)
    {
        if (item == null)
            throw new ArgumentException("Product group is null");

        if ((await GetAllAsync()).Exists(x => x.Name == item.Name))
            throw new ArgumentException($"Artykuł o nazwie {item.Name} już istnieje");

        await _repozitory.AddAsync(item);
        return item;
    }

    public async Task<Product> SetImage(Product product, byte[] imgBytes)
    {
        if (product == null)
            return null;
        if (imgBytes == null)
            return null;

        WareHouseImage image = null;

        if ((product?.Images?.Count ?? 0) == 0)
        {
            product.Images = new List<WareHouseImage>();
            image = new WareHouseImage()
            {
                ID = Guid.NewGuid(),
                Name = product.Name,
                CreatedAt = DateTime.Now,
                Lp = 0,
                Tag = "#Product",
                Img = imgBytes
            };
            await _imageService.SaveAsync();
            await _imageService.AddAsync(image);
            product.Images.Add(image);

        }
        else
        {
            image = product.Images.FirstOrDefault();
            image.Img = imgBytes;
        }
        Update(product);
        await _imageService.SaveAsync();
        return product;
    }

    public override async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {

        if (await ExistOnWareHouse(id))
            return false;

        return await base.DeleteAsync(id, cancellationToken);
    }

    public async Task<bool> ExistOnWareHouse(Guid id) => await _wareHouseItemService.ExistProduct(id);

    #endregion
}
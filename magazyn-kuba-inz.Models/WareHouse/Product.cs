using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models;

[Table("Product")]
public class Product : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public double Price { get; set; }

    public Guid ID_Status { get; set; }

    public ProductStatus? Status { get; set; }

    public Guid ID_Group { get; set; }

    public ProductGroup? Group { get; set; }

    public Guid ID_Supplier { get; set; }

    public Supplier? Supplier { get; set; }

    public List<WareHouseItem>? WareHouseItems { get; set; }

    public List<WareHouseImage>? Images { get; set; }

    public List<StorageItem>? StorageItemCollection { get; set; }

    public List<OrderProduct>? OrderItems { get; set; }

    public static Product Get()
    {
        return new Product()
        {
            ID = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Name = "",
            Description = "",
            Lp = 1
        };
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Warehouse.Models.Attribute;

namespace Warehouse.Models;

[Table("ProductStatus")]
public class ProductStatus : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }

    [FilterColumn]
    public string? Name { get; set; }

    public List<Product>? Products { get; set; }

    public static ProductStatus Get()
    {
        return new ProductStatus()
        {
            ID = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Name = "",
            Lp = 1
        };
    }
}
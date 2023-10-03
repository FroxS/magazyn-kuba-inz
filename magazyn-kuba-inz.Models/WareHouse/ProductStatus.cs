using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("ProductStatus")]
public class ProductStatus : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }

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
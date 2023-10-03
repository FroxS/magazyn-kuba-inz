using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("ProductGroup")]
public class ProductGroup :BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }

    public virtual string? Name { get; set; }

    public virtual string? Description { get; set; }

    public List<Product>? Products { get; set; }

    public static ProductGroup Get()
    {
        return new ProductGroup()
        {
            ID = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Name = "",
            Description = "",
            Lp = 1
        };
    }

}
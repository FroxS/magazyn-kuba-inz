using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

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
}
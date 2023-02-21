using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("ProductGroup")]
public class ProductGroup
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public List<Product> Products { get; set; }
}

using magazyn_kuba_inz.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("Product")]
public class Product
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public double Price { get; set; }

    public uint Count { get; set; }

    public ProductStatus Status { get; set; } = ProductStatus.Active;

    public Guid Id_Group { get; set; }
    public ProductGroup Group { get; set; }

    public List<StorageItemCollection> StorageItemCollections { get; set; }
}
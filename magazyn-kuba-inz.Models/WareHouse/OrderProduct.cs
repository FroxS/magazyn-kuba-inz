using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("OrderProduct")]
public class OrderProduct : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }

    public string? Name { get; set; }
    public Guid ID_Order { get; set; }
    public Order? Order { get; set; }
    public Guid ID_Product { get; set; }
    public Product? Product { get; set; }
    public Guid ID_StorageItem { get; set; }
    public StorageItemCollection? StorageItem { get; set; }
}
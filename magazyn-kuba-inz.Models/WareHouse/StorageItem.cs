using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("StorageItem")]
public class StorageItem : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }
    public string? Position { get; set; }
    public Guid ID_StorageUnit { get; set; }
    public StorageUnit? StorageUnit { get; set; }
    public Guid ID_Rack { get; set; }
    public Rack? Rack { get; set; }
    public List<StorageItemCollection>? Items { get; set; }
}
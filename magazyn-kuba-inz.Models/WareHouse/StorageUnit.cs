using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("StorageUnit")]
public class StorageUnit : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }
    public string? Name { get; set; }
    public double MaxWeight { get; set; }
    public double MaxHeight { get; set; }
    public double MaxWidth { get; set; }
    public double MaxDepth { get; set; }
    public List<StorageItem>? StorageItems { get; set; }
}
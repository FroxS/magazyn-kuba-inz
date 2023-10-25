using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models;

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
    public double SizeOfRack { get; set; }
    public List<StorageItemPackage>? Packages { get; set; }
}
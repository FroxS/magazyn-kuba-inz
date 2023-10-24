using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models;

[Table("StorageItemPackage")]
public class StorageItemPackage : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }
    public int Flor { get; set; }
    public Guid ID_StorageUnit { get; set; }
    public StorageUnit? StorageUnit { get; set; }
    public Guid ID_Rack { get; set; }
    public Rack? Rack { get; set; }
    public List<StorageItem>? Items { get; set; }

    public static StorageItemPackage Get()
    {
        return new StorageItemPackage()
        {
            ID = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Lp = 1
        };
    }
}
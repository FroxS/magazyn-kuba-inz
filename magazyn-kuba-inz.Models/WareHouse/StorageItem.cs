using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models;

[Table("StorageItem")]
public class StorageItem : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }
    public virtual Guid? ID_Package { get; set; }
    public virtual StorageItemPackage? Package { get; set; }

    public virtual Guid ID_Product { get; set; }
           
    public virtual Product? Product { get; set; }

    public virtual Guid ID_State { get; set; }

    public virtual ItemState? State { get; set; }

    public virtual Guid? ID_OrderItem { get; set; }
    public virtual OrderProduct? OrderItem { get; set; }

    public static StorageItem Get()
    {
        return new StorageItem()
        {
            ID = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Lp = 1
        };
    }
}
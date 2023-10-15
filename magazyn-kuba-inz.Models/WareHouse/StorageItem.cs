using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models;

[Table("StorageItem")]
public class StorageItem : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }
    public Guid ID_StorageItem { get; set; }
    public StorageItemPackage? Package { get; set; }
    public Guid ID_Product { get; set; }
    public Product? Product { get; set; }
    public Guid ID_OrderItem { get; set; }
    public OrderProduct? OrderItem { get; set; }
}
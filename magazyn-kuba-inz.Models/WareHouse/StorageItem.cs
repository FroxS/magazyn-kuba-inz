using magazyn_kuba_inz.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("StorageItem")]
public class StorageItem
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    public Guid Id_StorageUnit { get; set; }

    public StorageUnit StorageUnit { get; set; }

    public string Position { get; set; }

    public StorageItemStatus Status { get; set; }

    public Guid Id_RackPosition { get; set; }

    public RackPosition RackPosition { get; set; }

    public List<StorageItemCollection> StorageItems { get; set; }

    public List<OrderElement> OrderElements { get; set; }

}
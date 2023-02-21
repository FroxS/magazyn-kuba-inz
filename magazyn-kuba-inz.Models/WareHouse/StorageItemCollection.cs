
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("StorageItemCollection")]
public class StorageItemCollection
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    public Guid Id_Product { get; set; }

    public Product Product { get; set; }

    public Guid Id_StorageItem { get; set; }

    public StorageItem StorageItem { get; set; }
}
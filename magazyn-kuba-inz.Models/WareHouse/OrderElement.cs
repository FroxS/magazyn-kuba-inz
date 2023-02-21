
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("OrderElement")]
public class OrderElement
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    public Guid Id_Order { get; set; }

    public Order Order { get; set; }

    public int Count { get; set; }

    public Guid Id_StorageItem { get; set; }

    public StorageItem StorageItem { get; set; }
}
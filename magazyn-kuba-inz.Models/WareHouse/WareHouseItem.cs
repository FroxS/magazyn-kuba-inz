using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("WareHouseItem")]
public class WareHouseItem : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }

    public int Count { get; set; }

    public Guid ID_Product { get; set; }

    public Product? Product { get; set; }

    public Guid ID_State { get; set; }

    public ItemState? State { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("ItemState")]
public class ItemState : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }

    public string? Name { get; set; }

    public List<WareHouseItem>? Items { get; set; }

    public static ItemState Get()
    {
        return new ItemState()
        {
            ID = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Name = "",
            Lp = 1
        };
    }
}
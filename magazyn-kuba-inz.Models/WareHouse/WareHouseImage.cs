using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("Image")]
public class WareHouseImage : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }

    public string Name { get; set; }

    public string Tag { get; set; }

    public byte[] Img { get; set; }

    // Nawigacyjne właściwości
    public List<Product>? Products { get; set; }
}

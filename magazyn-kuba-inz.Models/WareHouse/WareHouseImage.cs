using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Warehouse.Models.Attribute;

namespace Warehouse.Models;

[Table("Image")]
public class WareHouseImage : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }

    [FilterColumn]
    public string Name { get; set; }

    public string Tag { get; set; }

    public byte[] Img { get; set; }

    // Nawigacyjne właściwości
    public List<Product>? Products { get; set; }
}

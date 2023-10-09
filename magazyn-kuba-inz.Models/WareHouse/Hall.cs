using magazyn_kuba_inz.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("Hall")]
public class Hall : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }
    public string Name { get; set; }
    public byte[] Data { get; set; }
    public List<Rack>? Racks { get; set; }
}
using magazyn_kuba_inz.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("Rack")]
public class Rack : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }
    public int Corridor { get; set; }
    public int Flors { get; set; }
    public double Width { get; set; }
    public double Heigth { get; set; }
    public double Deepth { get; set; }
    public EDir Direction { get; set; }
    public int AmountSpace { get; set; } = 2;
    public List<StorageItemPackage>? StorageItems { get; set; }
    public Guid ID_Hall { get; set; }
    public Hall? Hall { get; set; }
}
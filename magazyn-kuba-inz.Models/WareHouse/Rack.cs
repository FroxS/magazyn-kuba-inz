using magazyn_kuba_inz.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("Rack")]
public class Rack
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    public int Number { get; set; }

    public int Corridor { get; set; }

    public int Flors { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public double Depth { get; set; }

    public Direction Direction { get; set; }

    public uint Space_amount { get; set; }

    public List<RackPosition> Rack_Positions { get; set; }

}
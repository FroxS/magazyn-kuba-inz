using magazyn_kuba_inz.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("RackPosition")]
public class RackPosition
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    public Guid Id_Rack { get; set; }

    public Rack Rack { get; set; }

    public uint Flor { get; set; }

    public uint Position { get; set; }

    public List<StorageItem> StorageItems { get; set; }
}
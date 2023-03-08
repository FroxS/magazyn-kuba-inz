using System.ComponentModel.DataAnnotations;

namespace magazyn_kuba_inz.Models.WareHouse;

public abstract class BaseEntity
{
    [Key]
    [Required]
    public abstract Guid ID { get; set; }

    public DateTime CreatedAt { get; set; }
}
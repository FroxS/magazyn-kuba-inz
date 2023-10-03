using magazyn_kuba_inz.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("Order")]
public class Order : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }

    public string? Name { get; set; }
    public Guid ID_User { get; set; }
    public User? User { get; set; }
    public DateTime RealizationDate { get; set; }
    public double Cost { get; set; }
    public List<OrderProduct>? Items { get; set; }
}
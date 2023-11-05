using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models;

[Table("Order")]
public class Order : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }
    public string? Name { get; set; }
    public Guid ID_User { get; set; }
    public User? User { get; set; }
    public double Margin { get; set; }
    public byte[]? OrderWay { get; set; }
    public DateTime RealizationDate { get; set; }
    public List<OrderProduct>? Items { get; set; }
}
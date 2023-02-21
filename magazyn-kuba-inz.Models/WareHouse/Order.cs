
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace magazyn_kuba_inz.Models.WareHouse;

[Table("Order")]
public class Order
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid Id_User { get; set; }

    public User User { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime RealizationDate { get; set; }

    public double Cost { get; set; }

    public List<OrderElement> Items { get; set; }
}
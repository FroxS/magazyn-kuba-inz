using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models;

[Table("OrderProduct")]
public class OrderProduct : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }
    public string? Name { get; set; }
    public Guid ID_Order { get; set; }
    public Order? Order { get; set; }
    public Guid ID_Product { get; set; }
    public Product? Product { get; set; }
    public Guid ID_StorageItem { get; set; }
    public StorageItem? StorageItem { get; set; }

    public static OrderProduct Get()
    {
        return new OrderProduct()
        {
            ID = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Modified = DateTime.Now,
            Name = "",
            Lp = 1
        };
    }
}
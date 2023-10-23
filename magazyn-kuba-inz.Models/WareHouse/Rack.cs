using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Warehouse.Models.Enums;

namespace Warehouse.Models;

[Table("Rack")]
public class Rack : BaseEntity
{
    [Key]
    [Required]
    public override Guid ID { get; set; }
    public virtual int Corridor { get; set; }
    public virtual int Flors { get; set; }
    public virtual double Width { get; set; }
    public virtual double Heigth { get; set; }
    public virtual double Deepth { get; set; }
    public virtual EDir Direction { get; set; }
    public virtual int AmountSpace { get; set; } = 2;
    public virtual List<StorageItemPackage>? StorageItems { get; set; }
    public virtual Guid ID_Hall { get; set; }
    public virtual Hall? Hall { get; set; }
}
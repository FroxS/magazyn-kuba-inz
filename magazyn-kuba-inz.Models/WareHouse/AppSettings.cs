using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models;


[Table("AppSettings")]
public sealed class AppSettings : BaseEntity
{
    public override Guid ID { get; set; }
    public string? Data { get; set; }
}


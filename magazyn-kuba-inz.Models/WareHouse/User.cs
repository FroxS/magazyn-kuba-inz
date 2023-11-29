using System.ComponentModel.DataAnnotations.Schema;
using Warehouse.Models.Attribute;
using Warehouse.Models.Enums;
using Warehouse.Models.Interfaces;

namespace Warehouse.Models;


[Table("User")]
public sealed class User : BaseEntity, IUser
{
    public override Guid ID { get; set; }
    public string? Login { get; set; }
    [FilterColumn]
    public string? Name { get; set; }
    public bool Active { get; set; }
    public string? PasswordSalt { get; set; }
    public string? PasswordHash { get; set; }
    public string? Email { get; set; }
    public EUserType Type { get; set; }
    public string? Image { get; set; }
    public List<Order>? Orders { get; set; }

    public static User Get()
    {
        return new User()
        {
            ID = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Active = false,
            Type = EUserType.Employee_WareHouse,
            Lp = 1
        };
    }
}


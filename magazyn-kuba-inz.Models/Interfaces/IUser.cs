using Warehouse.Models.Enums;

namespace Warehouse.Models.Interfaces
{
    public interface IUser
    {
        Guid ID { get; }
        string? Login { get; }
        string? Name { get; }
        EUserType Type { get; }
        string? Image { get; }
        bool Active { get; }
        string Email { get; set; }
    }
}
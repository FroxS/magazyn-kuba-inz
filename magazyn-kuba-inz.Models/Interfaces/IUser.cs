using magazyn_kuba_inz.Models.Enums;

namespace magazyn_kuba_inz.Models.Interfaces
{
    public interface IUser
    {
        Guid Id { get; }
        string? Login { get; }
        string? Name { get; }
        UserType Type { get; }
        byte[] Image { get; }
    }
}
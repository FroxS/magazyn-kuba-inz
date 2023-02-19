using magazyn_kuba_inz.Models.Enums;
using magazyn_kuba_inz.Models.Interfaces;

namespace magazyn_kuba_inz.Models;

public class User : IUser
{
    public Guid Id { get; set; }
    public string? Login { get; set; }
    public string? Name { get; set; }
    public UserType Type { get; set; }
    public byte[] Image { get; set; }

}


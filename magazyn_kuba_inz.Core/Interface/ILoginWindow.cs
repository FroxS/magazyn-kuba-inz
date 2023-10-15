using Warehouse.Models.Interfaces;

namespace Warehouse.Core.Interface;

public interface ILoginWindow : IWindow
{
    IUser? GetUser();
}

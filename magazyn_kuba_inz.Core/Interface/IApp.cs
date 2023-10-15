using System.Windows.Threading;
using Warehouse.Models.Interfaces;
using Warehouse.Models.Enums;
using Warehouse.Core.Resources;

namespace Warehouse.Core.Interface;

public interface IApp
{
    IUser? User { get; }
    INavigation Navigation { get; }
    bool IsTaskRunning { get; set; }
    bool IsAdmin { get; }
    void Run();
    bool IsUserLogin();
    void LogOut();
    void ShowSilentMessage(string message, EMessageType type = EMessageType.Warning);
    IDialogService GetDialogService();
    IInnerDialogService GetInnerDialogService();
    Task<IUser> LoginAsync(LoginResource user);
    Task Register(RegisterResource user);
    void Exit();
    Dispatcher GetDispather();
    S GetService<S, T>() where T : class where S : IBaseService<T>;
}

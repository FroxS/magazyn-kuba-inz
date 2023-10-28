using System.Windows.Threading;
using Warehouse.Models.Interfaces;
using Warehouse.Models.Enums;
using Warehouse.Core.Resources;
using Warehouse.Models;
using System.Windows;

namespace Warehouse.Core.Interface;

public interface IApp
{
    User? User { get; }
    INavigation Navigation { get; }
    bool IsTaskRunning { get; set; }
    bool IsAdmin { get; }
    Window? MainWindow { get; }
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
    S GetService<S, T>() where T : BaseEntity where S : IBaseService<T>;

    S GetService<S>() where S : IBaseService<BaseEntity>;
    void CatchExeption(System.Exception ex);

    
}

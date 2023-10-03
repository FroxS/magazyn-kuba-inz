using magazyn_kuba_inz.Core.Resources;
using magazyn_kuba_inz.Core.Service.Dialog;
using magazyn_kuba_inz.Models.Enums;
using magazyn_kuba_inz.Models.Interfaces;
using System.Windows.Threading;

namespace magazyn_kuba_inz.Core.Service.Interface;

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

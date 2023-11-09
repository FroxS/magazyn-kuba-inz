using System.Windows.Threading;
using Warehouse.Models.Interfaces;
using Warehouse.Models.Enums;
using Warehouse.Core.Resources;
using Warehouse.Models;
using System.Windows;
using Warehouse.EF;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Core.Interface;

public interface IApp
{
    User? User { get; }
    INavigation Navigation { get; }
    bool IsTaskRunning { get; set; }
    bool IsAdmin { get; }
    Window? MainWindow { get; }
    DbContext Database { get; }
    Task Run();
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

    S GetService<S>();
    void CatchExeption(System.Exception ex);
    void ReloadDatabase();

    void SetTheme(bool dark = true);

}

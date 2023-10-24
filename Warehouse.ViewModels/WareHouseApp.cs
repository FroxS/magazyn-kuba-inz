using Warehouse.ViewModel.Service;
using Warehouse.Models.Enums;
using Warehouse.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Warehouse.Core.Interface;
using System.Windows.Threading;
using Warehouse.Core.Resources;
using Warehouse.Service;
using Warehouse.Core.Exeptions;
using Warehouse.Core.Helpers;
using Warehouse.Service.Interface;

namespace Warehouse.ViewModel;

public class WareHouseApp : BaseViewModel, IApp
{
    #region Private Properties

    private readonly INavigation nav;

    private readonly System.Windows.Application app;

    private static IServiceProvider _services;

    private readonly IUserService userService;

    #endregion

    #region Public Properties

    public IUser? User { get; private set; }

    public bool IsAdmin => User?.Type == EUserType.Admin;

    public INavigation Navigation => nav;

    public static IInnerDialogService InnerDialog => _services.GetRequiredService<IInnerDialogService>();

    public static IDialogService Dialog => _services.GetRequiredService<IDialogService>();

    #endregion

    #region Constructors

    public WareHouseApp(INavigation nav, System.Windows.Application app, IServiceProvider services, IUserService userService)
    {
        this.nav = nav;
        this.app = app;
        _services = services;
        this.userService = userService;
    }

    #endregion

    #region Public Methods

    public Dispatcher GetDispather() => _services.GetRequiredService<Dispatcher>();

    public void Run()
    {
        //var register = services.GetRequiredService<IRegisterWindow>();
        //register.ShowDialog();
        bool? flag = false;
        bool test = false;
        //if(false)
        if (System.Diagnostics.Debugger.IsAttached || test)
        {
            flag = true;
            //var task = Task.Run(async () => await Register(new RegisterResource("admin","admin@wp.pl", "admin")));
            var task = Task.Run(async () => await LoginAsync(new LoginResource("admin", "admin")));
            task.GetAwaiter().GetResult();
        }
        else
        {
            _services.GetRequiredService<ILoginWindow>();
            ILoginWindow login = _services.GetRequiredService<ILoginWindow>();
            if (login == null)
                throw new Exception("Brak okna login");

            flag = login.ShowDialog();
        }

        if (flag == false)
        {
            app.Shutdown();
            return;
        }
        if (User == null)
        {
            Run();
        }
        else
        {
            IMainWindow window = _services.GetRequiredService<IMainWindow>();
            if (window == null)
                throw new Exception("Brak okna głównego");

            app.MainWindow = window as Window;
            Navigation.SetPage(Warehouse.Models.Page.EApplicationPage.WareHouseCreator);
            window.Show();
        }
    }

    public void ShowSilentMessage(string message, EMessageType type = EMessageType.Warning)
    {
        _services.GetRequiredService<MessageService>().AddMessage(message, type);
    }

    public IDialogService GetDialogService() => _services.GetRequiredService<IDialogService>();

    public IInnerDialogService GetInnerDialogService() => _services.GetRequiredService<IInnerDialogService>();

    public S GetService<S,T>() where T : class where S: IBaseService<T> => _services.GetRequiredService<S>();

    public bool IsUserLogin()
    {
        throw new NotImplementedException();
    }

    public void Exit()
    {
        app.Shutdown();
    }

    public void LogOut()
    {
        throw new NotImplementedException();
    }

    public async Task<IUser> LoginAsync(LoginResource user)
    {
        User = await userService.Login(user);
        if (!User.Active)
        {
            User = null;
            throw new DataException("This user is not active");
        }
        return User;
    }

    public async Task Register(RegisterResource user)
    {
        if (user == null)
            throw new ArgumentException("User is empty");

        if (user.Login.Length < 5)
            throw new DataException("Minimalna liczba to 5", user, "Login");

        if (!UserHelper.IsValidEmail(user?.Email ?? ""))
            throw new DataException("Email jest nieprawidłowy", user, "Email");

        await userService.Register(user);
    }

    public void CatchExeption(System.Exception ex)
    {
        ShowSilentMessage("Wystąpił bład aplikacji", EMessageType.Error);

#if DEBUG
        GetDialogService().ShowError(ex);
#endif
    }

#endregion
}

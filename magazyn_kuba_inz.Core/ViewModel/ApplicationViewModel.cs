using magazyn_kuba_inz.Core.Exeptions;
using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Resources;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace magazyn_kuba_inz.Core.ViewModel;

public class ApplicationViewModel : BaseViewModel, IApp
{
    #region Private Properties

    private readonly INavigation nav;

    private readonly Application app;

    private readonly IServiceProvider services;

    private readonly IUserService userService;

    #endregion


    #region Public Properties

    public IUser? User { get; private set; }

    public bool IsAdmin => User?.Type == magazyn_kuba_inz.Models.Enums.UserType.Admin;

    #endregion

    #region Constructors

    public ApplicationViewModel(INavigation nav, Application app, IServiceProvider services, IUserService userService)
    {
        this.nav = nav;
        this.app = app;
        this.services = services;
        this.userService = userService;
    }

    #endregion

    #region Public Methods

    public void Run()
    {
        //var register = services.GetRequiredService<IRegisterWindow>();
        //register.ShowDialog();
        bool? flag = false;
        if (System.Diagnostics.Debugger.IsAttached)
        { 
            flag = true;
            //var task = Task.Run(async () => await Register(new RegisterResource("admin","admin@wp.pl", "admin")));
            var task = Task.Run(async () => await LoginAsync(new LoginResource("admin", "admin")));
            task.GetAwaiter().GetResult();
        }
        else
        {
            services.GetRequiredService<ILoginWindow>();

            ILoginWindow login = services.GetRequiredService<ILoginWindow>();

            if (login == null)
                throw new Exception("Brak okna login");


            flag = login.ShowDialog();
        }

        
        if(flag == false)
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
            IMainWindow window = services.GetRequiredService<IMainWindow>();
            if (window == null)
                throw new Exception("Brak okna głównego");

            app.MainWindow = window as Window;
            window.Show();
        }
    }

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
        if(!User.Active)
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
            throw new DataException("Minimalna liczba to 5",user,"Login");

        if (!UserHelper.IsValidEmail(user?.Email ?? ""))
            throw new DataException("Email jest nieprawidłowy", user, "Email");
        
        await userService.Register(user);
    }

    #endregion
}

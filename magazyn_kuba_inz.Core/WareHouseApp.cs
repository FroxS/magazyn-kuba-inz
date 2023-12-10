using Warehouse.Models.Enums;
using Warehouse.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Warehouse.Core.Interface;
using System.Windows.Threading;
using Warehouse.Core.Resources;
using Warehouse.Core.Exeptions;
using Warehouse.Core.Helpers;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using Warehouse.EF;
using Warehouse.Theme;
using Warehouse.Core.Models.Settings;

namespace Warehouse;

public class WareHouseApp : ObservableObject, IApp
{
    #region Private Properties
    private INavigation nav => GetService<INavigation>();

    private readonly System.Windows.Application app;

    private static IServiceProvider _services;
    private IUserService userService => _services.GetRequiredService<IUserService>();

    private bool isTaskRunning = false;

    private DbContext _database;

    private IDbContextFactory<WarehouseDbContext> _databaseFactory;

    public ISplashScreen _splashScreen;

    #endregion

    #region Public Properties

    public User? User { get; private set; }

    public bool IsAdmin => User?.Type == EUserType.Admin;

    public INavigation Navigation => nav;

    public static IInnerDialogService InnerDialog => _services.GetRequiredService<IInnerDialogService>();

    public static IDialogService Dialog => _services.GetRequiredService<IDialogService>();

    public static IApp App => _services.GetRequiredService<IApp>();

    public Window? MainWindow { get; private set; }

    public virtual bool IsTaskRunning { get => isTaskRunning; set { isTaskRunning = value; OnPropertyChanged(nameof(IsTaskRunning)); } }

    public DbContext Database => _database;

    public bool CanEditData { get; private set; }

    #endregion

    #region Constructors

    public WareHouseApp(Application app, IServiceProvider services)
    {
        this.app = app;
        _services = services;
        _databaseFactory = services.GetRequiredService<IDbContextFactory<WarehouseDbContext>>();
        ReloadDatabase();
        RelayCommand.DefaultActionOnError = (ex) => CatchExeption(ex);

    }

    #endregion

    #region Public Methods

    public UserSettings GetUserSettings()
    {
        var settings = GetService<UserSettings>();
        settings.Load();
        return settings;
    }

    public Dispatcher GetDispather() => _services.GetRequiredService<Dispatcher>();

    public async Task Run()
    {
        SetTheme(GetUserSettings()?.ColorScheme ?? ColorScheme.Dark);
        bool? flag = false;
        bool test = true;
        if (System.Diagnostics.Debugger.IsAttached && test)
        {
            flag = true;
            await LoginAsync(new LoginResource("admin", "admin"));
        }


        if (!IsUserLogin())
        {
            _services.GetRequiredService<ILoginWindow>();
            ILoginWindow login = _services.GetRequiredService<ILoginWindow>();
            if (login == null)
                throw new Exception(Core.Properties.Resources.ErrorLoginWindowNotExist);

            MainWindow = login as Window;
            CloseSplashForm();
            flag = login.ShowDialog();
        }

        if (flag == false)
        {
            app.Shutdown();
            return;
        }
        if (User == null)
        {
            await Run();
        }
        else
        {
            ClearSilentMessage();
            IMainWindow window = _services.GetRequiredService<IMainWindow>();
            if (window == null)
                throw new Exception(Core.Properties.Resources.ErrorMainWindowNotExist);
            MainWindow = app.MainWindow = window as Window;
            Navigation.SetPage(Warehouse.Models.Page.EApplicationPage.DashBoard);

            CloseSplashForm();
            window.Show();
        }
    }

    public void ShowSilentMessage(string message, EMessageType type = EMessageType.Warning)
    {
        _services.GetRequiredService<IMessageService>().AddMessage(message, type);
    }

    public void ClearSilentMessage()
    {
        _services.GetRequiredService<IMessageService>().Clear() ;
    }

    public IDialogService GetDialogService() => _services.GetRequiredService<IDialogService>();

    public IInnerDialogService GetInnerDialogService() => _services.GetRequiredService<IInnerDialogService>();

    public S GetService<S,T>() where T : BaseEntity where S: IBaseService<T> => _services.GetRequiredService<S>();

    public S GetService<S>() => _services.GetRequiredService<S>();

    public bool IsUserLogin() => User != null;

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
        try
        {
            User = await userService.Login(user);
            if (!User.Active)
            {
                User = null;
                throw new DataException(Core.Properties.Resources.ErrorUserIsNotActive);
            }
        }catch(System.Exception ex)
        {
            User = null;
            CatchExeption(ex);
        }
        
        return User;
    }

    public async Task Register(RegisterResource user)
    {
        if (user == null)
            throw new ArgumentException(Core.Properties.Resources.UserIsEmpty);

        if (user.Login.Length < 5)
            throw new DataException($"{Core.Properties.Resources.ErrorMaxLength} 5", user, nameof(User.Login));

        if (!UserHelper.IsValidEmail(user?.Email ?? ""))
            throw new DataException(Core.Properties.Resources.EmailisIncorrect, user, nameof(User.Email));

        await userService.Register(user);
        await userService.SaveAsync();
    }

    public void CatchExeption(Exception ex, bool showMessage = true)
    {
        ShowSilentMessage(Core.Properties.Resources.ApplicationErrorMessage, EMessageType.Error);

#if DEBUG
        GetDialogService().ShowError(ex);
#endif
    }

    public void ReloadDatabase()
    {
        if (_database == null)
        {
            _database = _databaseFactory.CreateDbContext();

        }
        else
        {
            _database.Dispose();
            _database = _databaseFactory.CreateDbContext();
        }
            
        //_database = _databaseFactory.CreateDbContext();  /// TODO: na razie wyłaczono ponieważ gdy już istniało jakieś powiązanie na innej karcie to nie mogło zostać zapisane 

    }

    private void CloseSplashForm()
    {
        if (_splashScreen != null)
        {
            _splashScreen.Close();
            _splashScreen = null;
        }
    }

    public void SetTheme(ColorScheme corloScheme = ColorScheme.Dark)
    {
        try
        {
            UserSettings settings = GetUserSettings();
            settings.ColorScheme = corloScheme;
            settings.Save();
            Theme.ColorsNavigation.ChangeColor( app, corloScheme );
        }catch(Exception ex)
        {
            CatchExeption(ex);
        }
    }

    #endregion
}

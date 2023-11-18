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
using System.Diagnostics;
using System.ComponentModel;

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

    public Window? MainWindow { get; private set; }

    public virtual bool IsTaskRunning { get => isTaskRunning; set { isTaskRunning = value; OnPropertyChanged(nameof(IsTaskRunning)); } }

    public DbContext Database => _database;

    public bool CanEditData { get; private set; }

    #endregion

    #region Constructors

    public WareHouseApp(System.Windows.Application app, IServiceProvider services)
    {
        this.app = app;
        _services = services;
        _databaseFactory = services.GetRequiredService<IDbContextFactory<WarehouseDbContext>>();
        ReloadDatabase();
    }

    #endregion

    #region Public Methods

    public Dispatcher GetDispather() => _services.GetRequiredService<Dispatcher>();

    public async Task Run()
    {
        bool? flag = false;
        bool test = true;
        if (System.Diagnostics.Debugger.IsAttached && test)
        {
            flag = true;
            await LoginAsync(new LoginResource("admin", "admin"));
        }
        else
        {
            _services.GetRequiredService<ILoginWindow>();
            ILoginWindow login = _services.GetRequiredService<ILoginWindow>();
            if (login == null)
                throw new Exception("Brak okna login");

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
            IMainWindow window = _services.GetRequiredService<IMainWindow>();
            if (window == null)
                throw new Exception("Brak okna głównego");
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

    public IDialogService GetDialogService() => _services.GetRequiredService<IDialogService>();

    public IInnerDialogService GetInnerDialogService() => _services.GetRequiredService<IInnerDialogService>();

    public S GetService<S,T>() where T : BaseEntity where S: IBaseService<T> => _services.GetRequiredService<S>();

    public S GetService<S>() => _services.GetRequiredService<S>();

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

    public void ReloadDatabase()
    {
        _database = _databaseFactory.CreateDbContext();
    }

    private BackgroundWorker waitingWorker = new BackgroundWorker();

    private void CloseSplashForm()
    {
        if (_splashScreen != null)
        {
            // Zamknij splash screen
            _splashScreen.Close();
            _splashScreen = null;
        }
    }

    public void SetTheme(bool dark = true)
    {
        string resThemeName = "DarkTheme";
        if (!dark)
            resThemeName = "LightTheme";

        string dictionaryNameToChange = "Theme";
        ResourceDictionary targetDictionary = null;
        foreach (var mergedDictionary in app.Resources.MergedDictionaries)
        {
            if (mergedDictionary is ResourceDictionary dictionary && dictionary.Source != null)
            {
                if (dictionary.Source.OriginalString.Contains(dictionaryNameToChange))
                {
                    targetDictionary = dictionary;
                    break;
                }
            }
        }

        // Sprawdź, czy istnieje słownik o nazwie "Themes"
        if (targetDictionary != null)
        {
            // Teraz możesz podmienić ten słownik na inny, na przykład "DarkTheme.xaml"
            targetDictionary.Source = new Uri($"/Warehouse.Theme;component/{resThemeName}.xaml", UriKind.Relative);
        }
        else
        {
            Debugger.Break();
        }
    }

    #endregion
}

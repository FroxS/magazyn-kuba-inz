using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace magazyn_kuba_inz.Core.ViewModel;

public class ApplicationViewModel : BaseViewModel, IApp
{
    #region Private Properties

    private readonly INavigation nav;

    private readonly Application app;

    private readonly IServiceProvider services;

    #endregion


    #region Public Properties

    public IUser? User { get; private set; }

    #endregion

    #region Constructors

    public ApplicationViewModel(INavigation nav, Application app, IServiceProvider services)
    {
        this.nav = nav;
        this.app = app;
        this.services = services;
    }

    #endregion

    #region Public Methods

    public void Run()
    {
        ILoginWindow login = services.GetRequiredService<ILoginWindow>();

        if (login == null)
            throw new Exception("Brak okna login");

        
        bool? flag = login.ShowDialog();
        if(flag == false)
        {
            app.Shutdown();
            return;
        }
        IUser? user = login.GetUser();
        if (user == null)
        {
            Run();;
        }
        else
        {
            Login(user);
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

    public void Login(IUser user)
    {
        User = user;
    }

    #endregion
}

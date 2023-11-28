using Warehouse.Core.Helpers;
using Warehouse.ViewModel.Service;
using Warehouse.Models.Page;
using System.Windows.Input;
using Warehouse.Core.Interface;
using System.Windows;
using Warehouse.Theme;

namespace Warehouse.ViewModel;

public class MainViewModel : BaseViewModel
{
    #region Private Properties

    private INavigation _nav => _app.GetService<INavigation>();

    private readonly IApp _app;

    private bool flag = false;

    #endregion

    #region Public Properties

    #endregion

    #region Command

    public ICommand NextPageCommand { get; private set; }

    public ICommand ChangeThemeCommand { get; private set; }

    public ICommand HelpCommand { get; private set; }

    public ICommand MinimizeCommand { get; private set; }

    public ICommand MaximizeCommand { get; private set; }

    public ICommand CloseCommand { get; private set; }

    public ICommand EditUserCommand { get; private set; }

    #endregion

    #region Constructors

    public MainViewModel(IApp app)
    {
        _app = app;
        NextPageCommand = new RelayCommand(() =>
        {
            _nav.SetPage(EApplicationPage.DashBoard);
        });

        ChangeThemeCommand = new RelayCommand(() =>
        {
            ColorScheme actual = ColorsNavigation.GetColorScheme();
            app.SetTheme(actual == ColorScheme.Dark ? ColorScheme.Light : ColorScheme.Dark);
        });

        MinimizeCommand = new RelayCommand<IWindow>((o) => { o.WindowState = WindowState.Minimized; });
        MaximizeCommand = new RelayCommand<IWindow>(maximizeWindow);
        CloseCommand = new RelayCommand(closeWindows);
        EditUserCommand = new RelayCommand(EditUser);
        HelpCommand = new RelayCommand(() => {
            MessageBox.Show("TODO: Help");
        });
    }

    #endregion


    #region Top bar methods

    private void maximizeWindow(IWindow o)
    {
        if (o == null)
            return;
        if (o.WindowState == WindowState.Maximized)
            o.WindowState = WindowState.Normal;
        else
            o.WindowState = WindowState.Maximized;
    }

    private void closeWindows()
    {
        _app.Exit();
    }

    private void EditUser()
    {
        if(_app.IsUserLogin())
        {
            _nav.OpenUser();
        }
    }

    #endregion
}


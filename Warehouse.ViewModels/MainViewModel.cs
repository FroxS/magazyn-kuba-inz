using Warehouse.Core.Helpers;
using Warehouse.ViewModel.Service;
using Warehouse.Models.Page;
using System.Windows.Input;
using Warehouse.Core.Interface;
using System.Windows;

namespace Warehouse.ViewModel;

public class MainViewModel : BaseViewModel
{
    #region Private Properties

    private readonly INavigation _nav;

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

    #endregion

    #region Constructors

    public MainViewModel(INavigation nav, IApp app)
    {
        _nav = nav;
        _app = app;
        NextPageCommand = new RelayCommand(() =>
        {
            nav.SetPage(EApplicationPage.DashBoard);
        });

        ChangeThemeCommand = new RelayCommand(() =>
        {
            app.SetTheme(flag);
            flag = !flag;
        });

        MinimizeCommand = new RelayCommand<IWindow>((o) => { o.WindowState = WindowState.Minimized; });
        MaximizeCommand = new RelayCommand<IWindow>(maximizeWindow);
        CloseCommand = new RelayCommand(closeWindows);
        HelpCommand = new RelayCommand(() => {
            MessageBox.Show("TODO: Help");
        });
    }

    #endregion

    #region Top bar methods

    // <summary>
    /// Method to maxmimize winodws
    /// </summary>
    private void maximizeWindow(IWindow o)
    {
        if (o == null)
            return;
        if (o.WindowState == WindowState.Maximized)
            o.WindowState = WindowState.Normal;
        else
            o.WindowState = WindowState.Maximized;
    }

    /// <summary>
    /// Method activate when windows is closing
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void closeWindows()
    {
        _app.Exit();
    }

    #endregion
}


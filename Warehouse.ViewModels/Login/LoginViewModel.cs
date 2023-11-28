using Warehouse.Core.Exeptions;
using Warehouse.Core.Helpers;
using Warehouse.Core.Resources;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using Warehouse.Models.Interfaces;
using Warehouse.ViewModel.Service;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Login;

public class LoginViewModel : BaseViewModel
{
    #region Private  Properties

    private readonly IApp app;
    private string? login;
    private string? password;

    #endregion

    #region Public Properties

    public IUser? User { get; private set; }

    [Required(ErrorMessageResourceName = "LoginIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources) )]
    public string? Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }

    [Required(ErrorMessageResourceName = "PasswordIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    public string? Password { get => password; set { password = value; OnPropertyChanged(nameof(Password)); } }

    #endregion

    #region Public Commands
    public ICommand MinimizeCommand { get; private set; }
    public ICommand ExitCommand { get; private set; }
    public ICommand LoginCommand { get; private set; }

    public ICommand RegisterCommand { get; private set; }

    #endregion

    #region Constructors

    public LoginViewModel(IApp app) : base()
    {
        LoginCommand = new RelayCommand<IWindow>(LoginUser, o => !IsTaskRunning);
        MinimizeCommand = new RelayCommand<IWindow>(minimize);
        ExitCommand = new RelayCommand<IWindow>(exit);
        RegisterCommand = new RelayCommand<IWindow>(OpenRegister);
        this.app = app;
    }

    #endregion

    #region Commands methods

    private void OpenRegister(IWindow window)
    {
        IRegisterWindow register = app.GetService<IRegisterWindow>();
        if(window is Window parent && register != null)
        {
            register.Owner = parent;
            var result = register.ShowDialog();
            if(result.HasValue && result.Value)
            {

            }
        }
    }

    private async void LoginUser(IWindow window)
    {
        try
        {
            User = null;
            _CanValidate = true;
            NotifyPropChanged(nameof(Login), nameof(Password));

            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
                return;

            try
            {
                IsTaskRunning = true;
                User = await app.LoginAsync(new LoginResource(Login, Password));
            }
            catch (DataException ex)
            {
                IsTaskRunning = false;
                if(ex.WrongProp != null)
                {
                    CustomMessage.Add(ex.WrongProp, ex.Message);
                    NotifyPropChanged(nameof(Login), nameof(Password));
                }
                else
                {
                    MessageBox.Show(ex.Message);
                } 
            }
            finally { IsTaskRunning = false; }

            if (User != null)
                window.DialogResult = true;
        }
        catch
        {
            MessageBox.Show("Nie udało się zalogować");
        }

    }

    private void minimize(IWindow window)
    {
        window.WindowState = WindowState.Minimized;
    }

    private void exit(IWindow window)
    {
        window.DialogResult = false;
    }

    #endregion
}

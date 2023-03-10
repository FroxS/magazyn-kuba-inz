using magazyn_kuba_inz.Core.Exeptions;
using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Resources;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel.Login;

public class LoginViewModel : BaseViewModel
{
    #region Private  Properties

    private readonly IApp app;
    private string? login;
    private string? password;

    #endregion

    #region Public Properties

    public IUser? User { get; private set; }

    [Required(ErrorMessage = "Login is required.")]
    public string? Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }

    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get => password; set { password = value; OnPropertyChanged(nameof(Password)); } }

    #endregion

    #region Public Commands
    public ICommand MinimizeCommand { get; private set; }
    public ICommand ExitCommand { get; private set; }
    public ICommand LoginCommand { get; private set; }

    #endregion

    #region Constructors

    public LoginViewModel(IApp app) : base()
    {
        LoginCommand = new RelayCommand<IWindow>(LoginUser, o => !IsTaskRunning);
        MinimizeCommand = new RelayCommand<IWindow>(minimize);
        ExitCommand = new RelayCommand(o => exit());
        this.app = app;
    }

    #endregion

    #region Commands methods

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
        window.WindowState = System.Windows.WindowState.Minimized;
    }

    private void exit()
    {
        app.Exit();
    }

    #endregion


}

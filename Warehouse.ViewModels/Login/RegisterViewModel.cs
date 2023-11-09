using Warehouse.Core.Exeptions;
using Warehouse.Core.Helpers;
using Warehouse.Core.Resources;
using Warehouse.ViewModel.Service;
using Warehouse.Models.Interfaces;
using Warehouse.Models.Validaton.Attribute;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using Warehouse.Core.Interface;
using Warehouse.Service.ErrorLog;

namespace Warehouse.ViewModel.Login;

public class RegisterViewModel : BaseViewModel
{
    #region Private  Properties

    private readonly IApp app;

    private string? login;

    private string? email;

    private string? passwordConfirm;

    private string? password;

    #endregion

    #region Public Properties

    public IUser? User { get; private set; }

    [Required(ErrorMessage = "Name is required.")]
    [MinLength(5, ErrorMessage = "Name is to short (5)")]
    [MaxLength(50, ErrorMessage = "No more than 50 characters")]
    public string? Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }

    [Required(ErrorMessage = "Name is required.")]
    [IsEmail("In not email.")]
    public string? Email { get => email; set { email = value; OnPropertyChanged(nameof(Email)); } }


    [Required(ErrorMessage = "Password is required.")]
    [MinLength(5, ErrorMessage = "Minimum length is 5")]
    [MaxLength(150, ErrorMessage = "Minimum length is 150")]
    public string? Password { get => password; set { password = value; OnPropertyChanged(nameof(Password)); } }

    [Required(ErrorMessage = "Confirm password is requied")]
    [MinLength(5, ErrorMessage = "Minimum length is 5")]
    [MaxLength(150, ErrorMessage = "Minimum length is 150")]
    public string? PasswordConfirm { get => passwordConfirm; set { passwordConfirm = value; OnPropertyChanged(nameof(PasswordConfirm)); } }

    #endregion

    #region Public Commands

    public ICommand MinimizeCommand { get; private set; }
    public ICommand ExitCommand { get; private set; }
    public ICommand RegisterCommand { get; private set; }

    #endregion

    #region Constructors

    public RegisterViewModel(IApp app) : base()
    {
        RegisterCommand = new RelayCommand<IWindow>(register);
        MinimizeCommand = new RelayCommand<IWindow>(minimize);
        ExitCommand = new RelayCommand<IWindow>(exit);
        this.app = app;
    }


    #endregion

    #region Commands methods

    private void register(IWindow obj)
    {
        _CanValidate = true;
        NotifyPropChanged(nameof(Login), nameof(Email), nameof(Password), nameof(PasswordConfirm));
        if ((Password != PasswordConfirm)
            || string.IsNullOrEmpty(Password)
            || string.IsNullOrEmpty(Login)
            || string.IsNullOrEmpty(Email))
        {
            return;
        }
        try
        {
            app.Register(new RegisterResource(Login, Email, Password));
            MessageBox.Show("Udało się stworzyc użytkownika");
        }
        catch (DataException ex)
        {
            MessageBox.Show(ex.Message);
        }
        catch (Exception ex)
        {
            ErrorLogService.ErrorLog(ex);
            MessageBox.Show("Wystąpił bład sprawdź logi");
        }
    }

    private void minimize(IWindow window)
    {
        window.WindowState = WindowState.Minimized;
    }

    private void exit(IWindow obj)
    {
        obj.DialogResult = false;
    }

    #endregion
}

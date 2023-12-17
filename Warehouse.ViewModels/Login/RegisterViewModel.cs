using Warehouse.Core.Exeptions;
using Warehouse.Core.Helpers;
using Warehouse.Core.Resources;
using Warehouse.ViewModel.Service;
using Warehouse.Models.Interfaces;
using Warehouse.Models.Attribute;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using Warehouse.Core.Interface;
using Warehouse.Models;

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

    public User? User { get; private set; }

    [Required(ErrorMessageResourceName = "LoginIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    [MinLength(5, ErrorMessage = "Name is to short (5)")]
    [MaxLength(50, ErrorMessage = "No more than 50 characters")]
    public string? Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }

    [Required(ErrorMessageResourceName = "EmailIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    [IsEmail(ErrorMessageResourceName = "WrongEmail", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    public string? Email { get => email; set { email = value; OnPropertyChanged(nameof(Email)); } }


    [Required(ErrorMessageResourceName = "PasswordIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    [MinLength(5, ErrorMessage = "Minimum length is 5")]
    [MaxLength(150, ErrorMessage = "Minimum length is 150")]
    public string? Password { get => password; set { password = value; OnPropertyChanged(nameof(Password)); } }

    [Required(ErrorMessageResourceName = "ConfirmPasswordIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    [MinLength(5, ErrorMessage = "Minimum length is 5")]
    [MaxLength(150, ErrorMessage = "Minimum length is 150")]
    public string? PasswordConfirm { get => passwordConfirm; set { passwordConfirm = value; OnPropertyChanged(nameof(PasswordConfirm)); } }

    public bool ExitOnSuccesfulRegister { get; set; } = false;
	public bool LoginOnSuccefulRegister { get; set; } = false;

	public IWindow Window { get; set; }

	#endregion

	#region Public Commands

	public ICommand MinimizeCommand { get; private set; }
    public ICommand ExitCommand { get; private set; }
    public ICommand RegisterCommand { get; private set; }

    #endregion

    #region Constructors

    public RegisterViewModel(IApp app) : base()
    {
        RegisterCommand = new AsyncRelayCommand<IWindow>(register);
        MinimizeCommand = new RelayCommand(minimize);
        ExitCommand = new RelayCommand(() => exit(false));
        this.app = app;
    }


    #endregion

    #region Commands methods

    private async Task register(IWindow obj)
    {
        _CanValidate = true;
        NotifyPropChanged(nameof(Login), nameof(Email), nameof(Password), nameof(PasswordConfirm));
        User = null;
        if ((Password != PasswordConfirm)
            || string.IsNullOrEmpty(Password)
            || string.IsNullOrEmpty(Login)
            || string.IsNullOrEmpty(Email))
        {
            return;
        }
        try
        {
            IsTaskRunning = true;
            User = (User)await app.Register(new RegisterResource(Login, Email, Password, Login));

            if (LoginOnSuccefulRegister)
            {
                IUserService userService = app.GetService<IUserService>();
				User.Active = true;
				userService.Update(User);
				userService.Save();
				await app.LoginAsync(new LoginResource(Login, Password));
			}
                

			IsTaskRunning = false;

            if (ExitOnSuccesfulRegister)
                exit(true);
			app.GetDialogService().ShowAlert(Core.Properties.Resources.SuccessfulCreatedUser);
			
		}
        catch (DataException ex)
        {
			IsTaskRunning = false;
			app.GetDialogService().ShowError(ex);
        }
        catch (Exception ex)
        {
			IsTaskRunning = false;
			app.CatchExeption(ex);
        }
        finally { IsTaskRunning = false; }
    }

    private void minimize()
    {
        Window.WindowState = WindowState.Minimized;
    }

    private void exit(bool flag = false)
    {
		Window.DialogResult = flag;
    }

    #endregion
}

using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using System.Windows.Input;
using Warehouse.Models.Enums;
using System.ComponentModel.DataAnnotations;
using Warehouse.Models.Attribute;
using Warehouse.Models;

namespace Warehouse.InnerDialog;

public class GetUserInnerDialogViewModel : BaseInnerDialogViewModel<User>
{
    #region Private fields

    private string _login = "";

    private string _password = "";

    private string _name = "";

    private string _email = "";

    private bool _active = false;

    private EUserType _userType = EUserType.Employee_WareHouse;

    #endregion

    #region Public properties

    [Required(ErrorMessageResourceName = "LoginIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    [MinLength(5, ErrorMessage = "Minimum length is 5")]
    [MaxLength(50, ErrorMessage = "Minimum length is 150")]
    public string Login 
    { 
        get => _login;
        set => SetProperty(ref _login, value);
    }

    [Required(ErrorMessageResourceName = "LoginIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    [MinLength(5, ErrorMessage = "Minimum length is 5")]
    [MaxLength(150, ErrorMessage = "Minimum length is 150")]
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    [Required(ErrorMessageResourceName = "LoginIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    [MinLength(2, ErrorMessage = "Minimum length is 2")]
    [MaxLength(200, ErrorMessage = "Minimum length is 200")]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value); 
    }

    [Required(ErrorMessageResourceName = "LoginIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    [IsEmail(ErrorMessageResourceName = "WrongEmail", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value); 
    }

    public bool Active
    {
        get => _active;
        set => SetProperty(ref _active, value);
    }

    public EUserType UserType
    {
        get => _userType;
        set => SetProperty(ref _userType, value); 
    }

    #endregion

    #region Commands

    public ICommand NoCommand { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public GetUserInnerDialogViewModel(IApp app) : base(app)
    {
        NoCommand = new RelayCommand(No);
        _CanValidate = true;
    }

    #endregion

    #region Command Methods

    protected override void Submit()
    {
        try
        {
            OnPropertyChanged(nameof(Login), nameof(Name), nameof(Password), nameof(Email), nameof(Active), nameof(UserType));
            var errors = GettErrors();
            if (errors.Count() > 0)
            {

                Message.AddMessage($"{Core.Properties.Resources.CheckData} : {string.Join(", ", errors)}", EMessageType.Warning);
                return;
            }
            Result = _app.GetService<IUserService>().GetUser(new Core.Resources.RegisterResource(Login,Email,Password,Name, UserType, Active));
            base.Submit();
        }
        catch(Exception ex) {
            Result = null;
            Message.AddMessage(Core.Properties.Resources.ApplicationError);
            _app.CatchExeption(ex,false);
        }

        
    }

    private void No()
    {
        try
        {
            Result = null;
            base.Submit();
        }
            catch(Exception ex) { _app.CatchExeption(ex); }
        }

    #endregion
}
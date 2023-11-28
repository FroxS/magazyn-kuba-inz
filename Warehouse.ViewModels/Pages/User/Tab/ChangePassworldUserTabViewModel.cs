using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using System.Windows.Input;
using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Warehouse.Models.Attribute;
using Warehouse.Models.Enums;
using Warehouse.Models.Interfaces;
using Warehouse.ViewModel.Service;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Warehouse.ViewModel.Pages;

public class ChangePassworldUserTabViewModel : BasePageViewModel
{
    #region Fields

    private readonly BasePageViewModel _parent;
    private readonly User _user;
    private IUserService _userService => Application.GetService<IUserService>();

    private bool _editMode;

    private string? _password;

    private string? _passwordConfirm;

    #endregion

    #region Properties

    public string? PasswordHash
    {
        get => _user.PasswordHash;
    }

    [Required(ErrorMessageResourceName = "PasswordIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    [MinLength(5, ErrorMessage = "Minimum length is 5")]
    [MaxLength(150, ErrorMessage = "Minimum length is 150")]
    public string? Password
    {
        get => _password;
        set { SetProperty(ref _password, value); }
    }

    [Required(ErrorMessageResourceName = "ConfirmPasswordIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    [MinLength(5, ErrorMessage = "Minimum length is 5")]
    [MaxLength(150, ErrorMessage = "Minimum length is 150")]
    public string? PasswordConfirm
    {

        get => _passwordConfirm;
        set { SetProperty(ref _passwordConfirm, value); }
    }

    public EUserType? Type
    {
        get => _user.Type;
        set { SetPropertyVal(_user, value, onChanged: () => { OnPropertyChanged(nameof(CanEditType)); }); }
    }

    [Required(ErrorMessageResourceName = "LoginIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    public string? Name
    {
        get => _user.Name;
        set { SetPropertyVal(_user, value); }
    }

    public bool IsAdmin
    {
        get => Application.IsAdmin;
    }

    public bool CanEditType
    {
        get => (Application.IsAdmin || _user.Type >= EUserType.Boss) && EditMode;
    }

    public bool EditMode
    {
        get => _editMode;
        set { SetProperty(ref _editMode, value, onChanged: () => { _CanValidate = value; OnPropertyChanged(nameof(CanEditType)); }); }
    }

    #endregion

    #region Commands

    public ICommand EditCommand { get; protected set; }
    public ICommand SaveCommand { get; protected set; }

    public ICommand SendMailCommand { get; protected set; }

    #endregion

    #region Constructors

    public ChangePassworldUserTabViewModel(BasePageViewModel parent): base(parent.Application)
    {
        _parent = parent;
        Page = Models.Page.EApplicationPage.User;
        if (!Application.IsUserLogin())
            throw new Exception("user is not login");

        if (Application.User == null)
            throw new Exception("User is not login");

        _user = Application.User;
        Init();
    }

    public ChangePassworldUserTabViewModel(BasePageViewModel parent, User user) : base(parent.Application)
    {
        
        Page = Models.Page.EApplicationPage.User;
        _user = (User)user;
        Init();
    }

    #endregion

    #region Private helpers

    protected void Init()
    {
        Title = Core.Properties.Resources.ChangePassword;
        EditCommand = new RelayCommand(() => { EditMode = true; });
        SaveCommand = new RelayCommand(Save, () => EditMode);
        SendMailCommand = new RelayCommand(() => SendMail());
    }

    private void SendMail()
    {
        
    }

    #endregion

    #region Public methods

    #endregion

    #region Command Methods
    private void Save()
    {
        try
        {
            _CanValidate = true;
            if ((Password != PasswordConfirm))
            {
                Application.ShowSilentMessage($"{Core.Properties.Resources.ErrorPasswordsAreNotTheSame}");
                return;
            }
            var errors = GettErrors();
            if(errors.Count() > 0)
            {
                Application.ShowSilentMessage($"{Core.Properties.Resources.CheckData} : {string.Join(", ", errors)}");
                return;
            }

            Application.ClearSilentMessage();

            IUserService service = _userService;
            if (!_userService.ChangePassword(new Core.Resources.ChangePassworldResource(_user.Login, Password)))
            {
                Application.ShowSilentMessage(Core.Properties.Resources.ErrorWhileSaving);
                return;
            }
            Application.ShowSilentMessage(Core.Properties.Resources.MessagePasswordChangedSuccessfully, EMessageType.Ok);
            EditMode = false;
        }
        catch(Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    #endregion
}

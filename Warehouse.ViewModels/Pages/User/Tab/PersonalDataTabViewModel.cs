using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Warehouse.Models.Attribute;
using Warehouse.Models.Enums;
using Warehouse.Models.Interfaces;
using Warehouse.ViewModel.Service;

namespace Warehouse.ViewModel.Pages;

public class PersonalDataTabViewModel : BasePageViewModel
{
    #region Fields

    private readonly User _user;
    private IUserService _userService => Application.GetService<IUserService>();

    private bool _editMode;

    #endregion

    #region Properties

    [Required(ErrorMessageResourceName = "LoginIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    public string? Name 
    {
        get => _user.Name;
        set { SetPropertyVal(_user, value); }
    }

    [Required(ErrorMessageResourceName = "EmailIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    [IsEmail(ErrorMessageResourceName = "WrongEmail", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    public string? Email
    {
        get => _user.Email;
        set { SetPropertyVal(_user, value); }
    }

    public bool Active
    {
        get => _user.Active;
        set { SetPropertyVal(_user, value); }
    }

    [Required(ErrorMessageResourceName = "LoginIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    public string? Login
    {
        get => _user.Login;
        set { SetPropertyVal(_user, value); }
    }

    public EUserType? Type
    {
        get => _user.Type;
        set { SetPropertyVal(_user, value, onChanged: () => { OnPropertyChanged(nameof(CanEditType));});}
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

    #endregion

    #region Constructors

    public PersonalDataTabViewModel(BasePageViewModel parent): base(parent.Application, parent)
    {
        Title = Core.Properties.Resources.PersonalData;
        Page = Models.Page.EApplicationPage.User;
        if (!Application.IsUserLogin())
            throw new Exception("user is not login");

        if (Application.User == null)
            throw new Exception("User is not login");

        _user = Application.User;
        Init();
    }

    public PersonalDataTabViewModel(BasePageViewModel parent, User user) : base(parent.Application, parent)
    {
        
        Page = Models.Page.EApplicationPage.User;
        _user = (User)user;
        Init();
    }

    #endregion

    #region Private helpers

    protected void Init()
    {
        EditCommand = new RelayCommand(() => { EditMode = true; });
        SaveCommand = new RelayCommand(Save, () => EditMode);
    }

    #endregion

    #region Public methods

    #endregion

    #region Command Methods
    private void Save()
    {
        try
        {
            var errors = GettErrors();
            if(errors.Count() > 0)
            {
                Application.ShowSilentMessage($"{Core.Properties.Resources.CheckData} : {string.Join(", ", errors)}");
                return;
            }
            Application.ClearSilentMessage();

            IUserService service = _userService;
            _userService.Update(_user);
            if (!_userService.Save())
            {
                Application.ShowSilentMessage(Core.Properties.Resources.ErrorWhileSaving);
                return;
            }
            EditMode = false;
        }
        catch(Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    #endregion
}

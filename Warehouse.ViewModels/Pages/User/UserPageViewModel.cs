using System.Collections.ObjectModel;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Warehouse.Models.Interfaces;
using Warehouse.ViewModel.Service;

namespace Warehouse.ViewModel.Pages;

public class UserPageViewModel : BasePageViewModel
{
    #region Fields

    private readonly User _user;
    private IUserService _userService => Application.GetService<IUserService>();

    private ObservableCollection<BasePageViewModel> _items;

    private BasePageViewModel _item;

    #endregion

    #region Properties

    public ObservableCollection<BasePageViewModel> Items
    {
        get => _items;
        set { SetProperty(ref _items, value); }
    }

    public BasePageViewModel Item
    {
        get => _item;
        set { SetProperty(ref _item, value); }
    }

    #endregion

    #region Constructors

    public UserPageViewModel(IApp app): base(app)
    {
        Page = Models.Page.EApplicationPage.User;
        if (!app.IsUserLogin())
            throw new Exception("user is not login");

        if (app.User == null)
            throw new Exception("User is not login");

        _user = app.User;
        Init();
    }

    public UserPageViewModel(IApp app, IUser user) : base(app)
    {
        Page = Models.Page.EApplicationPage.User;
        _user = (User)user;
        Init();
    }

    #endregion

    #region Private helpers

    protected void Init()
    {
        Items = new ObservableCollection<BasePageViewModel>() {
            new PersonalDataTabViewModel(this),
            new ChangePassworldUserTabViewModel(this),
        };
    }

    #endregion

}

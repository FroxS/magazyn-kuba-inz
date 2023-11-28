using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy UsersPage.xaml
/// </summary>
public partial class UsersPage : BaseControlPage<UsersPageViewModel>
{
    public UsersPage(UsersPageViewModel? vm) : base(vm)
    {
        InitializeComponent();
    }
    public UsersPage(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

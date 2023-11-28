using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy UserPage.xaml
/// </summary>
public partial class UserPage : BaseControlPage<UserPageViewModel>
{
    public UserPage(UserPageViewModel? vm) : base(vm)
    {
        InitializeComponent();
    }
    public UserPage(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

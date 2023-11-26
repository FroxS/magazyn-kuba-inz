using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy ProductsPage.xaml
/// </summary>
public partial class SettingsPage : BaseControlPage<SettingsPageViewModel>
{
    public SettingsPage(SettingsPageViewModel? vm) : base(vm)
    {
        InitializeComponent();
    }

    public SettingsPage(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

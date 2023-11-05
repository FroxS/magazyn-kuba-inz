using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy WayToOrderPage.xaml
/// </summary>
public partial class WayToOrderPage : BaseControlPage<WayToOrderPageViewModel>
{
    public WayToOrderPage(WayToOrderPageViewModel? vm) : base(vm)
    {
        InitializeComponent();
    }

    public WayToOrderPage(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

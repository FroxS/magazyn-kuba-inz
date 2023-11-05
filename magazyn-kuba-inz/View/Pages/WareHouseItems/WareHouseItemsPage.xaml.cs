using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy WareHouseItemsPage.xaml
/// </summary>
public partial class WareHouseItemsPage : BaseControlPage<WareHousePageViewModel>
{
    public WareHouseItemsPage(WareHousePageViewModel? vm) : base(vm)
    {
        InitializeComponent();
    }
    public WareHouseItemsPage(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

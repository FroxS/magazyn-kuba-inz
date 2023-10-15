using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy WareHouseCreatorPage.xaml
/// </summary>
public partial class WareHouseCreatorPage : BaseControlPage<WareHouseCreatorPageViewModel>
{ 
    public WareHouseCreatorPage(WareHouseCreatorPageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

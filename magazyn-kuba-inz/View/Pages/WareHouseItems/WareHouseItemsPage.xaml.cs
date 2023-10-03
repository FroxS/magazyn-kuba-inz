using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.View.Service;

namespace magazyn_kuba_inz.View.Pages;

/// <summary>
/// Logika interakcji dla klasy WareHouseItemsPage.xaml
/// </summary>
public partial class WareHouseItemsPage : BaseControlPage<WareHouseItemsPageViewModel>
{ 
    public WareHouseItemsPage(WareHouseItemsPageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.View.Service;

namespace magazyn_kuba_inz.View.Pages;

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

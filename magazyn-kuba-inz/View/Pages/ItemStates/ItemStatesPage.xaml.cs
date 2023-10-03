using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.View.Service;

namespace magazyn_kuba_inz.View.Pages;

/// <summary>
/// Logika interakcji dla klasy ItemStatesPage.xaml
/// </summary>
public partial class ItemStatesPage : BaseControlPage<ItemStatesPageViewModel>
{ 
    public ItemStatesPage(ItemStatesPageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

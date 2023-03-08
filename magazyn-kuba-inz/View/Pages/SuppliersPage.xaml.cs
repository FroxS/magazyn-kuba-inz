using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.View.Service;

namespace magazyn_kuba_inz.View.Pages;

/// <summary>
/// Logika interakcji dla klasy ProductsPage.xaml
/// </summary>
public partial class SuppliersPage : BasePage<SuppliersPageViewModel>
{ 
    public SuppliersPage(SuppliersPageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

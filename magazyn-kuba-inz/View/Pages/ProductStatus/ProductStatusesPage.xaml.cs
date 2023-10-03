using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.View.Service;

namespace magazyn_kuba_inz.View.Pages;

/// <summary>
/// Logika interakcji dla klasy ProductStatusesPage.xaml
/// </summary>
public partial class ProductStatusesPage : BaseControlPage<ProductStatusesPageViewModel>
{ 
    public ProductStatusesPage(ProductStatusesPageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

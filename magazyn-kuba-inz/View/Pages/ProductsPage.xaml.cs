using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.View.Service;

namespace magazyn_kuba_inz.View.Pages;

/// <summary>
/// Logika interakcji dla klasy ProductsPage.xaml
/// </summary>
public partial class ProductsPage : BasePage<ProductsPageViewModel>
{ 
    public ProductsPage(ProductsPageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

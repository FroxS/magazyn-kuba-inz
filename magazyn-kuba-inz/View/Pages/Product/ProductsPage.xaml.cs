using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.View.Service;

namespace magazyn_kuba_inz.View.Pages;

/// <summary>
/// Logika interakcji dla klasy ProductGroupListPage.xaml
/// </summary>
public partial class ProductsPage : BaseControlPage<ProductsPageViewModel>
{ 
    public ProductsPage(ProductsPageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

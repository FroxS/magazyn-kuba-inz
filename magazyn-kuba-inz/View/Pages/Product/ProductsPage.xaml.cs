using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

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

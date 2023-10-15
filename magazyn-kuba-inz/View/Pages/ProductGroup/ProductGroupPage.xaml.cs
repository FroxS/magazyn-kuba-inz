using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy ProductGroupListPage.xaml
/// </summary>
public partial class ProductGroupPage : BaseControlPage<ProductGroupsPageViewModel>
{ 
    public ProductGroupPage(ProductGroupsPageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

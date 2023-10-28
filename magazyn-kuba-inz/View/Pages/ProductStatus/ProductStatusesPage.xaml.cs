using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy ProductStatusesPage.xaml
/// </summary>
public partial class ProductStatusesPage : BaseControlPage<ProductStatusesPageViewModel>
{
    public ProductStatusesPage(ProductStatusesPageViewModel? vm) : base(vm)
    {
        InitializeComponent();
    }
    public ProductStatusesPage(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

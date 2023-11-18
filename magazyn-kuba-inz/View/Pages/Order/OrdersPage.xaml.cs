using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy OrdersPage.xaml
/// </summary>
public partial class OrdersPage : BaseControlPage<OrdersPageViewModel>
{
    public OrdersPage(OrdersPageViewModel? vm) : base(vm)
    {
        InitializeComponent();
    }
    public OrdersPage(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

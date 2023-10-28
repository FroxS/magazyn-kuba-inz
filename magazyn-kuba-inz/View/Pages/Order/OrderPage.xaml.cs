using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy ItemStatesPage.xaml
/// </summary>
public partial class OrderPage : BaseControlPage<OrderPageViewModel>
{
    public OrderPage(OrderPageViewModel? vm) : base(vm)
    {
        InitializeComponent();
    }
    public OrderPage(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

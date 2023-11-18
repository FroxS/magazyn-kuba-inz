using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy OrderEditAddPage.xaml
/// </summary>
public partial class OrderEditAddPage : BaseControlPage<OrderEditAddPageViewModel>
{
    public OrderEditAddPage(OrderEditAddPageViewModel? vm) : base(vm)
    {
        InitializeComponent();
    }
    public OrderEditAddPage(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy ProductsPage.xaml
/// </summary>
public partial class SuppliersPage : BaseControlPage<SuppliersPageViewModel>
{
    public SuppliersPage(SuppliersPageViewModel? vm) : base(vm)
    {
        InitializeComponent();

    }
    public SuppliersPage(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
        
    }
}

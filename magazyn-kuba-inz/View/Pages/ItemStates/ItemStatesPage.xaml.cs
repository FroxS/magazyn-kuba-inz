using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy ItemStatesPage.xaml
/// </summary>
public partial class ItemStatesPage : BaseControlPage<ItemStatesPageViewModel>
{
    public ItemStatesPage(ItemStatesPageViewModel? vm) : base(vm)
    {
        InitializeComponent();
    }
    public ItemStatesPage(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy StorageUnitsPage.xaml
/// </summary>
public partial class StorageUnitsPage : BaseControlPage<StorageUnitsPageViewModel>
{
    public StorageUnitsPage(StorageUnitsPageViewModel? vm) : base(vm)
    {
        InitializeComponent();
    }
    public StorageUnitsPage(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

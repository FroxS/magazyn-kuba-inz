using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.View.Service;

namespace magazyn_kuba_inz.View.Pages;

/// <summary>
/// Logika interakcji dla klasy StorageUnitsPage.xaml
/// </summary>
public partial class StorageUnitsPage : BaseControlPage<StorageUnitsPageViewModel>
{ 
    public StorageUnitsPage(StorageUnitsPageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

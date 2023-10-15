using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy RacksPage.xaml
/// </summary>
public partial class RacksPage : BaseControlPage<RacksPageViewModel>
{ 
    public RacksPage(RacksPageViewModel? vm):base(vm)
    {
        InitializeComponent();
        
    }
}

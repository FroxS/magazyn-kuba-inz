using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy RacksPage.xaml
/// </summary>
public partial class RacksPage : BaseControlPage<RacksPageViewModel>
{
    public RacksPage(RacksPageViewModel? vm) : base(vm)
    {
        InitializeComponent();

    }
    public RacksPage(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
        
    }
}

using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy RackPage.xaml
/// </summary>
public partial class RackPage : BaseControlPage<RackEditViewModel>
{
    public RackPage(RackEditViewModel? vm) : base(vm)
    {
        InitializeComponent();

    }
    public RackPage(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
        
    }
}

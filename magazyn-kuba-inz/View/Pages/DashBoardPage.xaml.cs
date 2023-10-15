using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy DashBoardPage.xaml
/// </summary>
public partial class DashBoardPage : BaseControlPage<DashBoardPageViewModel>
{ 
    public DashBoardPage(DashBoardPageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }

}

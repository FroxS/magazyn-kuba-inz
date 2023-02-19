using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.View.Service;

namespace magazyn_kuba_inz.View.Pages;

/// <summary>
/// Logika interakcji dla klasy DashBoardPage.xaml
/// </summary>
public partial class DashBoardPage : BasePage<DashBoardViewModel>
{ 
    public DashBoardPage(DashBoardViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

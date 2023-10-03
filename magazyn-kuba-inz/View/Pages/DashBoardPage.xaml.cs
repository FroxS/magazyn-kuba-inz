using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.View.Service;
using System.Windows.Controls;

namespace magazyn_kuba_inz.View.Pages;

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

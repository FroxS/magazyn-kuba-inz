using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.View.Service;
using System.Windows.Controls;
using System;
using magazyn_kuba_inz.Models.WareHouse;

namespace magazyn_kuba_inz.View.Pages;

/// <summary>
/// Logika interakcji dla klasy ProductsPage.xaml
/// </summary>
public partial class SuppliersPage : BaseControlPage<SuppliersPageViewModel>
{ 
    public SuppliersPage(SuppliersPageViewModel? vm):base(vm)
    {
        InitializeComponent();
        
    }
}

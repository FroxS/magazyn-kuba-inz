﻿using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy WareHouseItemsPage.xaml
/// </summary>
public partial class WareHouseItemsPage : BaseControlPage<WareHouseItemsPageViewModel>
{ 
    public WareHouseItemsPage(WareHouseItemsPageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

﻿using Warehouse.Core.Interface;
using Warehouse.View.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

/// <summary>
/// Logika interakcji dla klasy OrdersPage.xaml
/// </summary>
public partial class OrderPageView : BaseControlPage<OrderPageViewModel>
{
    public OrderPageView(OrderPageViewModel? vm) : base(vm)
    {
        InitializeComponent();
    }
    public OrderPageView(IBasePageViewModel? vm):base(vm)
    {
        InitializeComponent();
    }
}

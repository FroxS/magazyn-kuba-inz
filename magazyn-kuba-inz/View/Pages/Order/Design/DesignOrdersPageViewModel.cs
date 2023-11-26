using System;
using System.Collections.ObjectModel;
using Warehouse.Models;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

public class DesignOrdersPageViewModel : OrdersPageViewModel
{
    #region Singleton

    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static DesignOrdersPageViewModel Instance => new DesignOrdersPageViewModel();

    #endregion

    #region Constructor

    public DesignOrdersPageViewModel() : base(null,null)
    {
        IsTaskRunning = false;
        _CanValidate = true;
        Items = new ObservableCollection<OrderViewModel>()
        {
            new OrderViewModel(null, new Order() { Name = "ORD/2023/01", State = Models.Enums.EOrderState.Created, RealizationDate = DateTime.Now }, null),
            new OrderViewModel(null, new Order() { Name = "ORD/2023/02", State = Models.Enums.EOrderState.Reserved, RealizationDate = DateTime.Now }, null),
            new OrderViewModel(null, new Order() { Name = "ORD/2023/03", State = Models.Enums.EOrderState.Prepared, RealizationDate = DateTime.Now }, null),
        };

    }

    #endregion
}
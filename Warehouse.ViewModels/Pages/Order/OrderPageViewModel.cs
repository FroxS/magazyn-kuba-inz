using Warehouse.Models;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Service;

namespace Warehouse.ViewModel.Pages;

public class OrderPageViewModel : BasePageViewModel
{
    #region Private fields

    private readonly Order _order;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="app">Application service</param>
    public OrderPageViewModel(IApp app, Order order)
        : base(app)
    {
        Page = Models.Page.EApplicationPage.Order;
        _order = order;
        Title = order.Name;
    }

    #endregion

    #region Public methods

    #endregion

    #region Command Methods

    #endregion
}

using Warehouse.Models;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Service;

namespace Warehouse.ViewModel.Pages;

public class OrderEditAddPageViewModel : BasePageViewModel
{
    #region Private fields

    private readonly Order _order;
    private readonly IOrderService _service;
    private bool _toAdd = false;

    #endregion




    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="app">Application service</param>
    public OrderEditAddPageViewModel(IApp app, Order order)
        : base(app)
    {
        Page = Models.Page.EApplicationPage.EditAddOrder;
        _service = app.GetService<IOrderService>();
        if (order == null)
        {
            string newName = _service.GetNewOrderName();
            order = new Order() { Name = newName, RealizationDate = DateTime.Now.AddDays(10), ID_User = Application.User.ID };
            _toAdd = true;
        }
        
        _order = order;
        Title = order?.Name + (_toAdd ? "*": "");
    }

    #endregion

    #region Public methods

    public override void OnPageClose()
    {
        CanChangePage = true;
        if (_toAdd )
        {
            if (!_service.Add(_order))
            {
                CanChangePage = false;
                return;
            }

        }
        else
        {
            _service.Update(_order);
        }
        
        _service.Save();
    }

    #endregion

    #region Command Methods

    #endregion
}

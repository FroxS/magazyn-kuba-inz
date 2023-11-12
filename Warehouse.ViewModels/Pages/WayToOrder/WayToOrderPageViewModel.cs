using Warehouse.ViewModel.Service;
using Warehouse.Core.Interface;
using Warehouse.Core.Models;
using Warehouse.Core.Exeptions;
using Warehouse.Core.Helpers;
using Warehouse.Models;

namespace Warehouse.ViewModel.Pages;

public class WayToOrderPageViewModel : BasePageViewModel
{
    #region Private fields

    private IHallService _hallService;
    private HallObject _hall;
    private WayResult _way;
    private Order _order;

    #endregion

    #region Public properties

    public HallObject Hall
    {
        get => _hall;
        set { SetProperty(ref _hall, value, nameof(Hall)); }
    }

    public WayResult Way
    {
        get => _way;
        set { SetProperty(ref _way, value, nameof(Way)); }
    }

    public Order Order
    {
        get => _order;
        set { SetProperty(ref _order, value, nameof(Order)); }
    }

    #endregion

    #region Constructors

    public WayToOrderPageViewModel(IApp app, IHallService hallService, WayResult way, Order order) : base(app)
    {
        _hallService = hallService;
        Way = way;
        Order = order;
        Page = Models.Page.EApplicationPage.WayToOrder;
        Title = $"Droga - {order.Name}";
    }

    #endregion

    #region Public Methods

    public override void OnPageOpen()
    {
        try
        {
            base.OnPageOpen();
            var hall = _hallService.GetAll().FirstOrDefault();
            if (hall == null)
                throw new PageExeption("Skonfiguruj hale", Models.Page.EApplicationPage.WareHouseCreator);

            if (Way == null)
                throw new PageExeption("Nie wskazano drogi", Models.Page.EApplicationPage.DashBoard);

            if (Order == null)
                throw new PageExeption("Nie wskazano zamówienia", Models.Page.EApplicationPage.Order);

            Hall = _hallService.GetHallObject(hall.ID);
            
        }
        catch (Exception ex) { Application.GetDialogService().ShowAlert(ex.Message); }
        finally { Application.IsTaskRunning = false; }
    }

    #endregion
}

using Warehouse.Models;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Service;
using System.Windows.Input;
using Warehouse.Core.Helpers;
using Warehouse.Core.Models;
using Warehouse.Models.Enums;
using System.Collections.ObjectModel;

namespace Warehouse.ViewModel.Pages;

public class OrderEditAddPageViewModel : BasePageViewModel
{
    #region Private fields

    private readonly Order _order;
    private IOrderService _service => Application.GetService<IOrderService>();

    private bool _toAdd = false;

    private ObservableCollection<ITab> _items;

    private ITab _selectedItem;

    #endregion

    #region Public Properties

    public EOrderState State
    {
        get => _order.State;
        set
        {
            if (_order.State == value)
                return;
            _order.State = value;
            OnPropertyChanged(nameof(State), nameof(Reserved), nameof(Prepared));
        }
    }

    public bool Reserved => State == EOrderState.Reserved;

    public bool Prepared => State == EOrderState.Prepared;

    public bool Enabled => CanEdit();

    public ObservableCollection<ITab> Items
    {
        get => _items;
        set  { SetProperty(ref _items, value, nameof(Items)); }
    }

    public ITab SelectedItem
    {
        get => _selectedItem;
        set { SetProperty(ref _selectedItem, value, nameof(SelectedItem), () => value?.Load()); }
    }

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
        if (order == null)
        {
            string newName = _service.GetNewOrderName();
            order = new Order() { 
                Name = newName, 
                RealizationDate = DateTime.Now.AddDays(10), 
                ID_User = Application.User.ID, 
                Items = new List<OrderProduct>() 
            };
            _toAdd = true;
        }
        _order = order;
        Title = order?.Name + (_toAdd ? "*": "");
    }

    public OrderEditAddPageViewModel()
    {

    }

    #endregion

    #region Public methods

    public override void OnPageClose()
    {
        CanChangePage = true;
        if (_toAdd )
        {
            if(Application.GetDialogService().AskUser(Warehouse.Core.Properties.Resources.AskAddOrder + "?", Core.Properties.Resources.Question) == Models.Enums.EDialogResult.Yes)
            {
                if (!_service.Add(_order))
                {
                    CanChangePage = false;
                    return;
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            _service.Update(_order);
        }
        _service.Save();
    }

    public override void OnPageOpen()
    {
        Items = new ObservableCollection<ITab>();
        Items.Add(new OrderDataTabViewModel(this, Application));
        Items.Add(new OrderProductsTabViewModel(this, Application));
        if (_order.OrderWay != null)
        {
            var way = GetWay();
            Items.Add(new OrderWayTabViewModel(this, Application, way));
        }
        SelectedItem = Items.FirstOrDefault();
        
    }

    #endregion

    #region Command Methods

    internal void Reserv()
    {
        try
        {
            IWareHouseService whSer = Application.GetService<IWareHouseService>();
            string? message = null;
            if (!_service.Reserv(_order, whSer, ref message))
                Application.ShowSilentMessage(message ?? "Nie udało się zarezerować");
            else
            {
                Application.ShowSilentMessage("Udało się zarezerowwać", Models.Enums.EMessageType.Ok);
            }
            UpdateState();
        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    internal void SetAsPreapred()
    {
        try
        {
            IWareHouseService whSer = Application.GetService<IWareHouseService>();
            WayResult wayResult = GetWay();
            if (!_service.SetAsPrepared(_order, whSer))
                Application.ShowSilentMessage("Nie udało się przygotować");
            else
            {
                _service.SetWay(wayResult.GetPath(), _order);
                whSer.Save();
                _service.Save();
                Application.ShowSilentMessage("Udało się przygotować", Models.Enums.EMessageType.Ok);
            }
            UpdateState();
        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    internal WayResult GetWay()
    {
        IHallService hallService = Application.GetService<IHallService, Hall>();
        Hall hall = hallService.GetAll().First();
        HallObject hallObj = hallService.GetHallObject(hall.ID);
        IRackService rackService = Application.GetService<IRackService, Rack>();
        hallObj.IncludeProductToRacks(rackService);

        List<OrderProduct> items = _service.GetProducts(_order.ID) ?? new List<OrderProduct>();

        if (State == EOrderState.Prepared)
            return hallObj.GetPath(_service.GetWay(_order));
        else
            return hallObj.GetPath(items.Select(x => x.Product).ToList());

    }

    internal void UpdateState()
    {
        State = EOrderState.Created;
        if (_service.IsReserved(_order.ID))
        {
            State = EOrderState.Reserved;
            if (_service.IsPrepared(_order.ID))
                State = EOrderState.Prepared;
        }
    }

    protected bool CanEdit()
    {
        User? user = Application?.User;
        if (user != null)
        {
            if ((user.Type & (EUserType.Admin | EUserType.Boss | EUserType.Employee_Office)) != 0)
                return true;
        }
        return false;
    }

    public Order Get() => _order;

    #endregion
}

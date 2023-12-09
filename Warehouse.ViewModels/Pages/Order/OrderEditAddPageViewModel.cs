using Warehouse.Models;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Service;
using Warehouse.Core.Helpers;
using Warehouse.Core.Models;
using Warehouse.Models.Enums;
using System.Collections.ObjectModel;

namespace Warehouse.ViewModel.Pages;

public class OrderEditAddPageViewModel : BasePageViewModel
{
    #region Private fields

    private Guid _orderID;

    private Order _order;
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

    public bool ToAdd => _toAdd;

    public ObservableCollection<ITab> Items
    {
        get => _items;
        set  { SetProperty(ref _items, value, nameof(Items)); }
    }

    public ITab SelectedItem
    {
        get => _selectedItem;
        set {
            _selectedItem?.OnPageClose();
            SetProperty(ref _selectedItem, value, onChanged: () => value?.OnPageOpen()); 
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="app">Application service</param>
    public OrderEditAddPageViewModel(IApp app, Order? order = null, EOrderType type = EOrderType.WareHouse)
        : base(app)
    {
        Page = Models.Page.EApplicationPage.EditAddOrder;
        if (order == null)
        {
            string newName = _service.GetNewOrderName(type);
            order = new Order() {
                ID = Guid.NewGuid(),
                Name = newName,
                RealizationDate = DateTime.Now.AddDays(10),
                ID_User = Application.User.ID,
                Items = new List<OrderProduct>(),
                Type = type,
                State = type == EOrderType.WareHouse ? EOrderState.Created : EOrderState.DeliveryCreated,
            };
            _toAdd = true;
        }
        _order = order;
        Title = order?.Name + (_toAdd ? "*": "");
        _orderID = order.ID;
    }

    public OrderEditAddPageViewModel() { }

    #endregion

    #region Public methods

    public override void OnPageClose()
    {
        CanChangePage = true;
        if (_toAdd )
        {
            if(Application.GetDialogService().AskUser(Core.Properties.Resources.AskAddOrder + "?", Core.Properties.Resources.Question) == EDialogResult.Yes)
            {
                if (!_service.Add(_order))
                {
                    CanChangePage = false;
                    return;
                }
            }
            else
                return;
        }
        else
            _service.Update(_order);
        _service.Save();
        if (_toAdd)
            Application.ReloadDatabase(); //
    }

    public override void OnPageOpen()
    {
        if(!_toAdd)
            _order = _service.GetById(_orderID);

        Items = new ObservableCollection<ITab>();
        if(_order.Type == EOrderType.WareHouse)
            Items.Add(new OrderDataTabViewModel(this, Application));
        if (_order.Type == EOrderType.Supplier)
            Items.Add(new OrderFromSupplierDataTabViewModel(this, Application));
        
        if (!_toAdd)
        {
            Items.Add(new OrderProductsTabViewModel(this, Application));
            if (_order.OrderWay != null && _order.Type == EOrderType.WareHouse)
            {
                
                Items.Add(new OrderWayTabViewModel(this, Application, GetWay()));
            }
        } 
        if(SelectedItem == null)
            SelectedItem = Items.FirstOrDefault();
        
    }

    #endregion

    #region Command Methods

    internal WayResult GetWay()
    {
        IHallService hallService = Application.GetService<IHallService, Hall>();
        Hall hall = hallService.GetAll().First();
        HallObject hallObj = hallService.GetHallObject(hall.ID);
        IRackService rackService = Application.GetService<IRackService, Rack>();
        hallObj.IncludeProductToRacks(rackService);
        List<OrderProduct> items = _service.GetProducts(_order.ID) ?? new List<OrderProduct>();

        if (State >= EOrderState.Reserved)
            return hallObj.GetPath(_service.GetWay(_order));
        else
            return hallObj.GetPath(items.Select(x => x.Product).ToList());

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

    internal void SaveAdded()
    {
        try
        {
            if (_toAdd)
            {
                if (!_service.Add(_order))
                {
                    Application.ShowSilentMessage(Core.Properties.Resources.FailedToAddOrder);
                    return;
                }
                if (_service.Save())
                {
                    Title = _order?.Name;
                    _toAdd = false;
                    OnPageOpen();
                }
                else
                    Application.ShowSilentMessage(Core.Properties.Resources.FailedToSave);
            }
        }catch(Exception ex)
        {
            Application.CatchExeption(ex);
        }
        
    }

    #endregion
}

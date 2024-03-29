﻿using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using Warehouse.Core.Models;
using System.ComponentModel;
using System.Windows.Data;
using Warehouse.Models.Enums;

namespace Warehouse.ViewModel.Pages;

public class OrderViewModel : BaseEntityViewModel<Order>
{
    #region Private fields

    private ObservableCollection<OrderProduct> _items;

    private OrderProduct _selectedItem;

    private IOrderService _orderService => _service as IOrderService;

    private bool _reserved;

    private bool _prepared;

    protected string _searchString;

    #endregion

    #region Public Properties

    public ICollectionView Collection { get; private set; }

    [Required(ErrorMessageResourceName = "NameIsRequired", ErrorMessageResourceType = typeof(Core.Properties.Resources))]
    public string? Name
    {
        get => _entity.Name;
        set
        {
            if (_entity.Name == value)
                return;
            Saved = false;
            _entity.Name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public DateTime RealizationDate
    {
        get => _entity.RealizationDate;
        set
        {
            if (_entity.RealizationDate == value)
                return;
            Saved = false;
            _entity.RealizationDate = value;
            OnPropertyChanged(nameof(RealizationDate));
        }
    }

    public double Margin
    {
        get => _entity.Margin;
        set
        {
            if (_entity.Margin == value)
                return;
            Saved = false;
            _entity.Margin = value;
            OnPropertyChanged(nameof(Margin));
        }
    }

    public ObservableCollection<OrderProduct> Items
    {
        get => _items;
        set { SetProperty(ref _items, value, nameof(Items),
                () =>
                {
                    if (Collection != null)
                        Collection.Filter -= FilterCollection;
                    Collection = CollectionViewSource.GetDefaultView(value);
                    Collection.Filter += FilterCollection;
                }
            ); 
        }
    }

    public OrderProduct SelectedItem
    {
        get => _selectedItem;
        set { SetProperty(ref _selectedItem, value, nameof(SelectedItem)); }
    }

    public bool Reserved => _orderService.IsReserved(_entity.ID);

    public bool Prepared => _orderService.IsPrepared(_entity.ID);

    public EOrderState State
    {
        get => _entity.State;
    }

    public virtual string SearchString
    {
        get => _searchString;
        set
        {
            _searchString = value;
            OnPropertyChanged(nameof(SearchString));
            Collection.Refresh();
        }
    }

    #endregion

    #region Commands

    public ICommand AddCommand { get; protected set; }
    public ICommand DeleteCommand { get; protected set; }
    public ICommand ReservCommand { get; protected set; }
    public ICommand SetAsPreapredCommand { get; protected set; }

    #endregion

    #region Constructors

    public OrderViewModel(
        IOrderService service,
        Order order,
        IApp app
        ) : base(service, order,app)
    {
        _app = app;
        Saved = true;
        AddCommand = new RelayCommand(AddProduct, () => Enabled && !Reserved && !Prepared);
        DeleteCommand = new RelayCommand<OrderProduct>(DeleteProduct, (o) => o != null && Enabled && !Reserved && !Prepared);
        ReservCommand = new RelayCommand(Reserv, () => Enabled && !Reserved && !Prepared);
        SetAsPreapredCommand = new RelayCommand(SetAsPreapred, () => Enabled && !Prepared);
        LoadProducts();
    }

    #endregion

    #region Protected methods

    private bool FilterCollection(object value)
    {
        if (value is OrderProduct item && item != null && !string.IsNullOrEmpty(SearchString))
        {
            if (item.Name?.ToLower().Contains(SearchString.ToLower()) ?? true)
                return true;
            else
                return false;
        }
        else
            return true;
    }

    protected override string[] GetpropsNameToFireOnSave()
    {
        return new string[] {
            nameof(Name),
            nameof(Margin),
            nameof(RealizationDate),
            nameof(Items)
        };
    }

    private void Reserv()
    {
        try
        {
            IWareHouseService whSer = _app.GetService<IWareHouseService>();
            string? message = null;
            if (!_orderService.Reserv(_entity, whSer,ref message))
                _app.ShowSilentMessage(message ?? Core.Properties.Resources.FailedToReserved);
            else
            {
                _app.ShowSilentMessage(Core.Properties.Resources.SuccesfullReserved, Models.Enums.EMessageType.Ok);
            }
            OnPropertyChanged(nameof(Reserved), nameof(Prepared));
        }
        catch (Exception ex)
        {
            _app.CatchExeption(ex);
        }
    }

    private void SetAsPreapred()
    {
        try
        {
            IWareHouseService whSer = _app.GetService<IWareHouseService>();
            WayResult wayResult = GetWay();
            if (!_orderService.SetAsPrepared(_entity, whSer))
                _app.ShowSilentMessage( Core.Properties.Resources.FailedToPrepared);
            else
            {
                _orderService.SetWay(wayResult.GetPath(), _entity);
                whSer.Save();
                _service.Save();
                _app.ShowSilentMessage(Core.Properties.Resources.SuccesfullPrepared, Models.Enums.EMessageType.Ok);
            }
            OnPropertyChanged(nameof(Reserved), nameof(Prepared));
        }
        catch (Exception ex)
        {
            _app.CatchExeption(ex);
        }
    }

    private WayResult GetWay()
    {
        IHallService hallService = _app.GetService<IHallService, Hall>();
        Hall hall = hallService.GetAll().First();
        HallObject hallObj = hallService.GetHallObject(hall.ID);
        IRackService rackService = _app.GetService<IRackService, Rack>();
        hallObj.IncludeProductToRacks(rackService);

        if(Prepared)
            return hallObj.GetPath(_orderService.GetWay(_entity));
        else
            return hallObj.GetPath(Items.Select(x => x.Product).ToList());

    }

    private void LoadProducts()
    {
        if(_orderService != null)
        {
            Items = new ObservableCollection<OrderProduct>(_orderService.GetProducts(_entity?.ID ?? Guid.Empty));
            _entity.State = _orderService.GetState(_entity.ID);
        }
    }

    private void DeleteProduct(OrderProduct item)
    {
        try
        {
            if(_app.GetDialogService().AskUser($"{Core.Properties.Resources.AreYouSureToDelete} {item.Name} ?", Core.Properties.Resources.Removing) == EDialogResult.Yes)
            {
                _entity.Items.Remove(item);
                _service.Update(_entity);
                _service.Save();
                LoadProducts();
                _app.ShowSilentMessage(Core.Properties.Resources.SuccessfulRemoved, Models.Enums.EMessageType.Ok);
            }
            
        }
        catch (Exception ex)
        {
            _app.CatchExeption(ex);
        }
    }

    private void AddProduct()
    {
        try
        {
            Product product = _app.GetDialogService().GetProduct();
            if(product != null)
            {
                //_service.RefreshDbContext();
                OrderProduct orderProduct = OrderProduct.Get();
                orderProduct.ID_Product = product.ID;
                orderProduct.Name = product.Name;
                //orderProduct.Order = _entity;
                orderProduct.ID_Order = _entity.ID;
                if (_entity.Items == null)
                    _entity.Items = new List<OrderProduct>();

                IOrderProductService opService = _app.GetService<IOrderProductService,OrderProduct>();
                opService.Add(orderProduct);
                opService.Save();
                LoadProducts();
                _app.ShowSilentMessage(Core.Properties.Resources.Added, EMessageType.Ok);

            }
        }catch(Exception ex)
        {
            _app.CatchExeption(ex);
        }
    }

    #endregion

    #region Public methods

    public override string? Valid()
    {
        string message = null;

        string[] props = GetpropsNameToFireOnSave();

        foreach (string prop in props)
        {
            message = GettErrors(prop);
            if (!string.IsNullOrWhiteSpace(message))
                return message;
        }

        var taks = _orderService.GetAll(Get().Type);
        if (taks.Find(o => o.Name == Name && ID != o.ID) != null)
            return $"{Core.Properties.Resources.Name} {Name} {Core.Properties.Resources.ExistInDatabase.ToLower()}";

        return base.Valid();
    }

    #endregion
}

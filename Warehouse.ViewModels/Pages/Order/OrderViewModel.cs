using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Warehouse.Core.Helpers;
using Warehouse.Core.Interface;
using Warehouse.Core.Models;
using System.ComponentModel;
using System.Windows.Data;

namespace Warehouse.ViewModel.Pages;

public class OrderViewModel : BaseEntityViewModel<Order>
{

    #region Private fields

    private ObservableCollection<OrderProduct> _items;

    private OrderProduct _selectedItem;

    private IOrderService _orderService => _service as IOrderService;

    private IApp _app;

    private bool _reserved;

    private bool _prepared;

    protected string _searchString;

    #endregion

    #region Public Properties

    public ICollectionView Collection { get; private set; }

    [Required(ErrorMessage = "Name is required.")]
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

    public bool Reserved
    {
        get => _reserved;
        set { SetProperty(ref _reserved, value, nameof(Reserved)); }
    }

    public bool Prepared
    {
        get => _prepared;
        set { SetProperty(ref _prepared, value, nameof(Prepared)); }
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
    public ICommand GenerateWayCommand { get; protected set; }
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
        GenerateWayCommand = new RelayCommand(GenerateWay, () => Enabled);
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
                _app.ShowSilentMessage(message ?? "Nie udało się zarezerować");
            else
            {
                _app.ShowSilentMessage("Udało się zarezerowwać", Models.Enums.EMessageType.Ok);
            }
            Reserved = _orderService.IsReserved(_entity.ID);
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
                _app.ShowSilentMessage("Nie udało się przygotować");
            else
            {
                _orderService.SetWay(wayResult.GetPath(), _entity);
                whSer.Save();
                _service.Save();
                _app.ShowSilentMessage("Udało się przygotować", Models.Enums.EMessageType.Ok);
            }
            Reserved = _orderService.IsReserved(_entity.ID);
            Prepared = _orderService.IsPrepared(_entity.ID);
        }
        catch (Exception ex)
        {
            _app.CatchExeption(ex);
        }
    }

    private void GenerateWay()
    {
        try
        {
            WayResult result = GetWay();
            if (result == null)
                _app.ShowSilentMessage("Nie udało się wygererować drogi");
            else
            {
                _app.Navigation.AddPage(new WayToOrderPageViewModel(_app, _app.GetService<IHallService, Hall>(), result, _entity));
            }
        }
        catch(Exception ex)
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
            Reserved = _orderService.IsReserved(_entity.ID);
            Prepared = _orderService.IsPrepared(_entity.ID);
        }
    }

    private void DeleteProduct(OrderProduct item)
    {
        try
        {
            if(_app.GetDialogService().GetYesNoDialog($"Czy na pewno usunąć produkt {item.Name}?", "Usuwanie") == Models.Enums.EDialogResult.Yes)
            {
                _entity.Items.Remove(item);
                _service.Update(_entity);
                _service.Save();
                LoadProducts();
                _app.ShowSilentMessage("Pomyślnie usunięto produkt", Models.Enums.EMessageType.Ok);
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
            Product product = _app.GetDialogService().GetProduct("Wyszukaj produkt");
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
                _app.ShowSilentMessage("Dodano",Models.Enums.EMessageType.Ok);

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

        var taks = _service.GetAll();
        if (taks.Find(o => o.Name == Name && ID != o.ID) != null)
            return $"Nazwa {Name} juz istnieje w bazie danych";

        return base.Valid();
    }

    #endregion
}

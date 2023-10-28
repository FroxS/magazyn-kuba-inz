using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Warehouse.Service.Interface;
using System.Windows.Input;
using Warehouse.Core.Helpers;
using System.Windows.Controls;
using Warehouse.Service;
using Warehouse.Core.Interface;
using Warehouse.Core.Models;

namespace Warehouse.ViewModel.Pages;

public class OrderViewModel : BaseEntityViewModel<Order>
{

    #region Private fields

    private ObservableCollection<OrderProduct> _items;

    private OrderProduct _selectedItem;

    private IOrderService _orderService => _service as IOrderService;

    private IApp _app;

    #endregion

    #region Public Properties

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
        set
        {
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }

    public OrderProduct SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged(nameof(SelectedItem));
        }
    }

    #endregion

    #region Commands

    public ICommand AddCommand { get; protected set; }
    public ICommand DeleteCommand { get; protected set; }

    public ICommand CalcWayCommand { get; protected set; }

    #endregion

    #region Constructors
    public OrderViewModel(
        IOrderService service, 
        Order order,
        IApp app
        ) : base(service, order)
    {
        _app = app;
        Saved = true;
        LoadProducts();
        AddCommand = new RelayCommand(AddProduct);
        DeleteCommand = new RelayCommand<OrderProduct>(DeleteProduct);
        CalcWayCommand = new RelayCommand(CalcWay);
    }

    #endregion

    #region Protected methods

    protected override string[] GetpropsNameToFireOnSave()
    {
        return new string[] {
            nameof(Name),
            nameof(Margin),
            nameof(RealizationDate),
            nameof(Items)
        };
    }


    private void LoadProducts()
    {
        Items = new ObservableCollection<OrderProduct>(_orderService.GetProducts(_entity?.ID ?? Guid.Empty));
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

    private void CalcWay()
    {
        try
        {
            IHallService hallService = _app.GetService<IHallService, Hall>();
            Hall hall = hallService.GetAll().First();
            HallObject hallObj = hallService.GetHallObject(hall.ID);
            if(hallObj != null)
            {

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

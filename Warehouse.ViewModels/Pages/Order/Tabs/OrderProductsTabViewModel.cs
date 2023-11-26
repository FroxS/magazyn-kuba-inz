using Warehouse.Core.Interface;
using Warehouse.ViewModel.Pages;
using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.ComponentModel.DataAnnotations;
using Warehouse.Models.Enums;
using System.Windows.Input;
using Warehouse.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Warehouse.Core.Helpers;

namespace Warehouse.ViewModel.Pages;

public class OrderProductsTabViewModel : BasePageViewModel
{
    #region Private properties

    private Order _order => Parent.Get();

    private IOrderService _service => Application.GetService<IOrderService>();

    protected string _searchString;

    private ObservableCollection<OrderProduct> _items;

    private OrderProduct _selectedItem;

    #endregion

    #region Public properties

    public ICollectionView Collection { get; private set; }

    public string SearchString
    {
        get => _searchString;
        set { SetProperty(ref _searchString, value, nameof(SearchString), () => Collection.Refresh()); }
    }

    public ObservableCollection<OrderProduct> Items
    {
        get => _items;
        set
        {
            SetProperty(ref _items, value, nameof(Items),
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

    public OrderEditAddPageViewModel Parent { get; }

    #endregion

    #region Commands

    public ICommand AddCommand { get; protected set; }
    public ICommand DeleteCommand { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public OrderProductsTabViewModel(OrderEditAddPageViewModel parent , IApp app): base(app)
    {
        AddCommand = new RelayCommand(AddProduct, () => Parent.Enabled && Parent.State == EOrderState.Created);
        DeleteCommand = new RelayCommand<OrderProduct>(DeleteProduct, (o) => o != null && Parent.Enabled && Parent.State == EOrderState.Created);

        Parent = parent;
        Title = Warehouse.Core.Properties.Resources.Products;
    }

    #endregion

    #region Load

    public override void OnPageOpen()
    {
        if (_service != null)
        {

            Items = new ObservableCollection<OrderProduct>(_service.GetProducts(_order?.ID ?? Guid.Empty) ?? new List<OrderProduct>());
            Parent.UpdateState();
        }
    }

    #endregion

    #region Command methods

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

    private void AddProduct()
    {
        try
        {
            Product product = Application.GetDialogService().GetProduct();
            if (product != null)
            {
                //_service.RefreshDbContext();
                OrderProduct orderProduct = OrderProduct.Get();
                orderProduct.ID_Product = product.ID;
                orderProduct.Name = product.Name;
                //orderProduct.Order = _entity;
                orderProduct.ID_Order = _order.ID;
                if (_order.Items == null)
                    _order.Items = new List<OrderProduct>();

                IOrderProductService opService = Application.GetService<IOrderProductService, OrderProduct>();
                opService.Add(orderProduct);
                opService.Save();
                OnPageOpen();
                Application.ShowSilentMessage(Warehouse.Core.Properties.Resources.Added, Models.Enums.EMessageType.Ok);

            }
        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    private void DeleteProduct(OrderProduct item)
    {
        try
        {
            if (Application.GetDialogService().AskUser($"{Warehouse.Core.Properties.Resources.AskRemoveProduct} {item.Name}?", Warehouse.Core.Properties.Resources.Question) == Models.Enums.EDialogResult.Yes)
            {
                IOrderProductService opService = Application.GetService<IOrderProductService, OrderProduct>();
                opService.Delete(item.ID);
                opService.Save();
                OnPageOpen();
                Application.ShowSilentMessage(Core.Properties.Resources.RemovedProduct, Models.Enums.EMessageType.Ok);
            }

        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    #endregion

}
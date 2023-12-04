using Warehouse.Core.Interface;
using Warehouse.Models;
using Warehouse.Models.Enums;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Warehouse.Core.Helpers;

namespace Warehouse.ViewModel.Pages;

public class OrderProductsTabViewModel : BasePageSearchItemsViewModel<OrderProduct>
{
    #region Private properties

    private Order _order => Parent.Get();

    private IOrderService _service => Application.GetService<IOrderService>();


    #endregion

    #region Public properties

    public override OrderEditAddPageViewModel Parent => base.Parent as OrderEditAddPageViewModel;

    #endregion

    #region Commands

    public ICommand AddCommand { get; protected set; }
    public ICommand DeleteCommand { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public OrderProductsTabViewModel(OrderEditAddPageViewModel parent , IApp app): base(app, parent)
    {
        Title = Warehouse.Core.Properties.Resources.Products;
        AddCommand = new RelayCommand(AddProduct, () => Parent.Enabled && CanAdd());
        DeleteCommand = new RelayCommand<OrderProduct>(DeleteProduct, (o) => o != null && Parent.Enabled && CanAdd());  
    }

    #endregion

    #region Load

    public override void OnPageOpen()
    {
        if (_service != null)
        {
            Items = new ObservableCollection<OrderProduct>(_service.GetProducts(_order?.ID ?? Guid.Empty) ?? new List<OrderProduct>());
        }
    }

    #endregion

    #region Command methods

    protected override bool Filter(OrderProduct value, string search)
    {
        if (value.Name?.ToLower().Contains(SearchString?.ToLower() ?? "") ?? true)
            return true;
        else
            return base.Filter(value, search);
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

    #region Helpers

    private bool CanAdd()
    {
        if (_order == null)
            return false;

        if (_order.Type == EOrderType.WareHouse)
            return Parent.State == EOrderState.Created;

        if (_order.Type == EOrderType.Supplier)
            return Parent.State == EOrderState.DeliveryCreated;


        return false;
    }

    #endregion

}
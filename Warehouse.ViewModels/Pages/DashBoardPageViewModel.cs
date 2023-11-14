using Warehouse.Core.Helpers;
using Warehouse.ViewModel.Service;
using System.Windows;
using System.Windows.Input;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class DashBoardPageViewModel : BasePageViewModel
{
    #region Private Fields

    private int _countOfProducts = 0;

    private int _countOfUsers = 0;

    private int _countOfOrders = 0;

    private double[,] _orderLineChart = null;

    private readonly IOrderService _orderService;

    private readonly IProductService _productService;

    private readonly IUserService _userService;

    #endregion

    #region Public Properties

    public int CountOfProducts
    {
        get => _countOfProducts;
        set { SetProperty(ref _countOfProducts, value, nameof(CountOfProducts)); }
    }

    public int CountOfUsers
    {
        get => _countOfUsers;
        set { SetProperty(ref _countOfUsers, value, nameof(CountOfUsers)); }
    }

    public int CountOfOrders
    {
        get => _countOfOrders;
        set { SetProperty(ref _countOfOrders, value, nameof(CountOfOrders)); }
    }

    public double[,] OrderLineChart
    {
        get => _orderLineChart;
        set { SetProperty(ref _orderLineChart, value, nameof(OrderLineChart)); }
    }

    #endregion

    #region Command

    public ICommand AddNewOrderCommand { get; protected set; }

    #endregion

    #region Constructors

    public DashBoardPageViewModel(IApp app, IOrderService orderService, IProductService productService, IUserService userService) : base(app)
    {
        AddNewOrderCommand = new RelayCommand(() => app.Navigation.SetPage(Models.Page.EApplicationPage.Order));
        _orderService = orderService;
        _productService = productService;
        _userService = userService;
    }

    #endregion

    #region Private Methods

    public override async void OnPageOpen()
    {
        try
        {
            IsTaskRunning = true;

            CountOfProducts = (await _productService.GetAllAsync()).Count;
            CountOfOrders = (await _orderService.GetAllAsync()).Count;
            CountOfUsers = (await _userService.GetUsers()).Count;

            OrderLineChart = new double[,] {
                { 1, 8 },   //pon
                { 2, 15 },  //wt
                { 3, 25 },  //śr
                { 4, 20 },  //czw
                { 5, 14 },  //pt
            };

            IsTaskRunning = false;
        }
        catch(Exception ex) 
        {
            IsTaskRunning = false;
            Application.CatchExeption(ex);
        }finally
        {
            IsTaskRunning = false;
        }
        
    }

    #endregion
}

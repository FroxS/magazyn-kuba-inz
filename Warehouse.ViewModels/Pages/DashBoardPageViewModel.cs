using Warehouse.Core.Helpers;
using Warehouse.ViewModel.Service;
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
        AddNewOrderCommand = new RelayCommand(() => {
			Application.Navigation.AddPage(new OrderEditAddPageViewModel(Application));
		});
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
                { 1, 41 },
                { 2, 29 },
                { 3, 16 },
                { 4, 12 },
                { 5, 34 },
                { 6, 22 },
                { 7, 19 },
                { 8, 49 },
                { 9, 30 },
                { 10, 38 },
                { 11, 23 },
                { 12, 46 },
                { 13, 26 },
                { 14, 44 },
                { 15, 13 },
                { 16, 15 },
                { 17, 36 },
                { 18, 11 },
                { 19, 37 },
                { 20, 27 },
                { 21, 35 },
                { 22, 14 },
                { 23, 33 },
                { 24, 18 },
                { 25, 42 },
                { 26, 17 },
                { 27, 45 },
                { 28, 40 },
                { 29, 20 },
                { 30, 31 },
                { 31, 10 }
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

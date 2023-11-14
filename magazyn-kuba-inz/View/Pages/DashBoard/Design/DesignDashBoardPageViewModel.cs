using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

public class DesignDashBoardPageViewModel : DashBoardPageViewModel
{
    #region Singleton

    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static DesignDashBoardPageViewModel Instance => new DesignDashBoardPageViewModel();

    #endregion

    #region Constructor

    public DesignDashBoardPageViewModel() : base(null, null, null, null)
    {
        IsTaskRunning = false;
        _CanValidate = true;

        OrderLineChart = new double[,] { 
            { 1, 8 },   //pon
            { 2, 15 },  //wt
            { 3, 25 },  //śr
            { 4, 20 },  //czw
            { 5, 14 },  //pt
        };
    }

    #endregion
}
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
    }

    #endregion
}
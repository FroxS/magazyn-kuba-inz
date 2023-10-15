using Warehouse.ViewModel.Design;

namespace Warehouse.ViewModel.Login;

public class DesignLoginViewModel : LoginViewModel
{
    #region Singleton

    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static DesignLoginViewModel Instance => new DesignLoginViewModel();

    #endregion

    #region Constructor

    public DesignLoginViewModel() : base(null)
    {
        IsTaskRunning = false;
        _CanValidate = true;
    }

    #endregion
}
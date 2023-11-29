using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class OrderFromSupplierDataTabViewModel : OrderDataTabViewModel
{
    #region Public properties

    #endregion

    #region Commands

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public OrderFromSupplierDataTabViewModel(OrderEditAddPageViewModel parent , IApp app): base(parent, app)
    {
        
    }

    #endregion

    #region Command methods

    public override void OnPageOpen()
    {

        base.OnPageOpen();
    }

    #endregion
}
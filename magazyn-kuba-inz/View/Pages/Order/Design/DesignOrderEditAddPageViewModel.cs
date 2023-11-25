using System.Collections.ObjectModel;
using Warehouse.Models;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

public class DesignOrderEditAddPageViewModel : OrderEditAddPageViewModel
{
    #region Singleton

    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static DesignOrderEditAddPageViewModel Instance => new DesignOrderEditAddPageViewModel();

    #endregion

    #region Constructor

    public DesignOrderEditAddPageViewModel(): base(null, new Order() { Name = "ORD/12/3213" })
    {
        IsTaskRunning = false;
        _CanValidate = true;

        Items = new ObservableCollection<Core.Interface.ITab>()
        {
           new OrderDataTabViewModel(this,null),
           new OrderWayTabViewModel(this,null,null),
           new OrderProductsTabViewModel(this,null)

        };
        //SelectedItem = Items[0];



    }

    #endregion
}
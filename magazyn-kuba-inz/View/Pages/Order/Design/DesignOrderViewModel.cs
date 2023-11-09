using System.Collections.ObjectModel;
using Warehouse.Models;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

public class DesignOrderViewModel : OrderViewModel
{
    #region Singleton

    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static DesignOrderViewModel Instance => new DesignOrderViewModel();

    #endregion

    #region Constructor

    public DesignOrderViewModel() : base(null,new Order(),null)
    {
        IsTaskRunning = false;
        _CanValidate = true;

        Name = "ORD/2023/02";

        Items = new ObservableCollection<OrderProduct>()
        {
            new OrderProduct(){ Name = "Produkt 1", Product = new Product(){ Name= "Produkt 1" } },
            new OrderProduct(){ Name = "Produkt 2", Product = new Product(){ Name= "Produkt 2" } },
            new OrderProduct(){ Name = "Produkt 3", Product = new Product(){ Name= "Produkt 3" } }
        };

    }

    #endregion
}
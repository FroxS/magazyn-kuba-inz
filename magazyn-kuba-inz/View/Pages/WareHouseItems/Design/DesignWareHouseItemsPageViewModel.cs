using System.Collections.ObjectModel;
using Warehouse.Models;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

public class DesignWareHouseItemsPageViewModel : WareHousePageViewModel
{
    #region Singleton

    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static DesignWareHouseItemsPageViewModel Instance => new DesignWareHouseItemsPageViewModel();

    #endregion

    #region Constructor

    public DesignWareHouseItemsPageViewModel() : base()
    {
        //IsTaskRunning = false;
        _CanValidate = true;

        States = new ObservableCollection<ProtuctStateTabViewModel>()
        {
            new DesignProtuctStateTabViewModel(new ItemState(){State = Models.Enums.EState.Delivery, Name = "Delicery"}){ Title = "Delicery" },
            new DesignProtuctStateTabViewModel(new ItemState(){State = Models.Enums.EState.InStock, Name = "InStock"}){ Title = "InStock" },
            new DesignProtuctStateTabViewModel(new ItemState(){State = Models.Enums.EState.Available, Name = "Available"}){ Title = "Available" }
        };

        //Name = "ORD/2023/02";

        //Items = new ObservableCollection<OrderProduct>()
        //{
        //    new OrderProduct(){ Name = "Produkt 1", Product = new Product(){ Name= "Produkt 1" } },
        //    new OrderProduct(){ Name = "Produkt 2", Product = new Product(){ Name= "Produkt 2" } },
        //    new OrderProduct(){ Name = "Produkt 3", Product = new Product(){ Name= "Produkt 3" } }
        //};

    }

    #endregion
}
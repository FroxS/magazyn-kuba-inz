using System.Collections.ObjectModel;
using Warehouse.Models;
using Warehouse.ViewModel.Pages;

namespace Warehouse.View.Pages;

public class DesignProtuctStateTabViewModel : ProtuctStateTabViewModel
{
    #region Singleton

    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static DesignProtuctStateTabViewModel Instance => new DesignProtuctStateTabViewModel();

    #endregion

    #region Constructor

    public DesignProtuctStateTabViewModel() : base(new ItemState() { State = Models.Enums.EState.Available, Name = "Available" })
    {

    }

    public DesignProtuctStateTabViewModel(ItemState state) : base(state)
    {

    }

    #endregion
}
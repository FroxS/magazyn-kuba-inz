using Warehouse.Core.Interface;
using Warehouse.ViewModel.Pages;
using Warehouse.ViewModel.Service;
using Warehouse.Models;
using Warehouse.Core.Helpers;
using System.Windows.Input;
using System;
using Warehouse.Core.Models;

namespace Warehouse.ViewModel.Pages;

public class OrderWayTabViewModel : BasePageViewModel
{
    #region Private properties

    private Order _order => Parent.Get();

    private IOrderService _service => Application.GetService<IOrderService>();
    private IHallService _hallService => Application.GetService<IHallService>();

    private WayResult _way;

    private HallObject _hall;

    #endregion

    #region Public properties

    public HallObject Hall
    {
        get => _hall;
        set { 
            SetProperty(ref _hall, value, nameof(Hall)); 
        }
    }

    public WayResult Way
    {
        get => _way;
        protected set { 
            SetProperty(ref _way, value, nameof(Way)); 
        }
    }

    public OrderEditAddPageViewModel Parent { get; }

    #endregion

    #region Commands

    public ICommand ReloadWayCommand { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public OrderWayTabViewModel(OrderEditAddPageViewModel parent , IApp app, WayResult way) : base(app)
    {
        Parent = parent;
        UpdateWay(way);
        ReloadWayCommand = new RelayCommand(() => ReloadWay(), () => Parent.State == Models.Enums.EOrderState.Created);
        Title = Warehouse.Core.Properties.Resources.Way;
    }

    #endregion

    #region Public method

    public void UpdateWay(WayResult way)
    {
        try
        {
            if (way == null)
                return;
            var hall = _hallService.GetAll().FirstOrDefault();
            Way = way;
            if (hall == null)
                throw new Exception("Skonfiguruj hale");

            if (Way == null)
                throw new Exception("Nie wskazano drogi");

            if (_order == null)
                throw new Exception("Nie wskazano zamówienia");

            Hall = _hallService.GetHallObject(hall.ID);
        }
        catch (Exception ex) { Application.CatchExeption(ex); }  
    }

    #endregion


    #region Command methods

    private void ReloadWay()
    {
        try
        {
            WayResult result = Parent.GetWay();
            if (result == null)
                Application.ShowSilentMessage(Core.Properties.Resources.FailedToGenerateWay);
            else
                UpdateWay(result);

        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);
        }

    }

    #endregion

}
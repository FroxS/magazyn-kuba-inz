using Warehouse.Core.Interface;
using Warehouse.ViewModel.Pages;
using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.ComponentModel.DataAnnotations;
using Warehouse.Models.Enums;
using System.Windows.Input;
using Warehouse.Core.Helpers;
using System.Windows;

namespace Warehouse.ViewModel.Pages;

public class OrderDataTabViewModel : BasePageViewModel
{
    #region Private properties

    private Order _order => Parent.Get();

    private IOrderService _service => Application.GetService<IOrderService>();
    private double _totalPrice = 0;

    #endregion

    #region Public properties

    [Required(ErrorMessage = "Name is required.")]
    public string? Name
    {
        get => _order.Name;
        set
        {
            if (_order.Name == value)
                return;
            _order.Name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public DateTime RealizationDate
    {
        get => _order.RealizationDate;
        set
        {
            if (_order.RealizationDate == value)
                return;
            _order.RealizationDate = value;
            OnPropertyChanged(nameof(RealizationDate));
        }
    }

    public double Margin
    {
        get => _order.Margin;
        set
        {
            if (_order.Margin == value)
                return;
            _order.Margin = value;
            OnPropertyChanged(nameof(Margin));
            UpdatePrice();
        }
    }

    public double TotalPrice
    {
        get => _totalPrice;
        protected set { SetProperty(ref _totalPrice, value, nameof(TotalPrice)); }
    }

    public OrderEditAddPageViewModel Parent { get; }

    #endregion

    #region Commands

    public ICommand GenerateWayCommand { get; protected set; }
    public ICommand ReservCommand { get; protected set; }
    public ICommand SetAsPreapredCommand { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public OrderDataTabViewModel(OrderEditAddPageViewModel parent , IApp app): base(app)
    {
        Parent = parent;
        Title = Warehouse.Core.Properties.Resources.Data;
        GenerateWayCommand = new RelayCommand(() => GenerateWay());
        ReservCommand = new RelayCommand(() => Parent.Reserv(), () => Parent.State == EOrderState.Created);
        SetAsPreapredCommand = new RelayCommand(() => Parent.SetAsPreapred(), () => Parent.State == EOrderState.Reserved);
    }

    #endregion

    #region Command methods
        
    private void GenerateWay()
    {
        try
        {
            ITab? tabOrderway = Parent.Items.FirstOrDefault(x => x.GetType() == typeof(OrderWayTabViewModel));
            WayResult result = Parent.GetWay();
            if (result == null)
                throw new ArgumentException(Warehouse.Core.Properties.Resources.FailedToGenerateWay);

            if (tabOrderway == null)
            {
                tabOrderway = new OrderWayTabViewModel(Parent, Application, result);
                Parent.Items.Add(tabOrderway);
            }
            else
                ((OrderWayTabViewModel)tabOrderway).UpdateWay(result);

            Parent.SelectedItem = tabOrderway;


        }
        catch(Exception ex) { Application.CatchExeption(ex); }
        
    }

    public void UpdatePrice()
    {
        try
        {
            if(_service != null)
                TotalPrice = _service.TotalPrice(_order);
        }
        catch (Exception ex) { Application.CatchExeption(ex); }
    }

    public override void Load()
    {
        UpdatePrice();
    }

    #endregion
}
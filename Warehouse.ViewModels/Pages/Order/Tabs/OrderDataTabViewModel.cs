using Warehouse.Core.Interface;
using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.ComponentModel.DataAnnotations;
using Warehouse.Models.Enums;
using System.Windows.Input;
using Warehouse.Core.Helpers;
using Warehouse.Core.Models;

namespace Warehouse.ViewModel.Pages;

public class OrderDataTabViewModel : BasePageViewModel
{
    #region Private properties

    protected Order _order => Parent.Get();

    protected IOrderService _service => Application.GetService<IOrderService>();

    protected double _totalPrice = 0;

    #endregion

    #region Public properties

    [Required(ErrorMessageResourceName = "NameIsRequired")]
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

    public EOrderState State => _service.GetState(_order.ID);

    public double TotalPrice
    {
        get => _totalPrice;
        protected set { SetProperty(ref _totalPrice, value, nameof(TotalPrice)); }
    }

    public bool ToAdd { get; protected set; }

    public override OrderEditAddPageViewModel Parent => base.Parent as OrderEditAddPageViewModel;

    #endregion

    #region Commands

    public ICommand GenerateWayCommand { get; protected set; }
    public ICommand ReservCommand { get; protected set; }
    public ICommand SetAsPreapredCommand { get; protected set; }
    public ICommand SaveCommand { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public OrderDataTabViewModel(OrderEditAddPageViewModel parent , IApp app): base(app, parent)
    {
        Title = Core.Properties.Resources.Data;
        GenerateWayCommand = new RelayCommand(() => GenerateWay(), () => !ToAdd);
        ReservCommand = new RelayCommand(() => Reserv(), () => Parent.State == EOrderState.Created && !ToAdd);
        SetAsPreapredCommand = new RelayCommand(() => SetAsPreapred(), () => Parent.State == EOrderState.Reserved && !ToAdd);
        SaveCommand = new RelayCommand(() => Parent.SaveAdded(), () => ToAdd);
    }

    #endregion

    #region Command methods

    private void Reserv()
    {
        try
        {
            IWareHouseService whSer = Application.GetService<IWareHouseService>();
            string? message = null;
            WayResult wayResult = Parent.GetWay();
            if (!_service.Reserv(_order, whSer, ref message))
                Application.ShowSilentMessage(message ?? Core.Properties.Resources.FailedToReserved);
            else
            {
                _service.SetWay(wayResult.GetPath(), _order);
                if(_service.Save())
                    Application.ShowSilentMessage(Core.Properties.Resources.SuccesfullReserved, EMessageType.Ok);
                else
                    Application.ShowSilentMessage(Core.Properties.Resources.FailedToSave, EMessageType.Warning);
            }
            UpdateState();
        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    private void SetAsPreapred()
    {
        try
        {
            IWareHouseService whSer = Application.GetService<IWareHouseService>();
            WayResult wayResult = Parent.GetWay();
            if (!_service.SetAsPrepared(_order, whSer))
                Application.ShowSilentMessage(Core.Properties.Resources.FailedToPrepared);
            else
            {
                _service.SetWay(wayResult.GetPath(), _order);
                whSer.Save();
                _service.Save();
                Application.ShowSilentMessage(Core.Properties.Resources.SuccesfullPrepared, EMessageType.Ok);
            }
            UpdateState();
        }
        catch (Exception ex)
        {
            Application.CatchExeption(ex);
        }
    }

    private void UpdateState()
    {
        Parent.State = EOrderState.Created;
        if (_service.IsReserved(_order.ID))
        {
            Parent.State = EOrderState.Reserved;
            if (_service.IsPrepared(_order.ID))
                Parent.State = EOrderState.Prepared;
        }
    }

    private void GenerateWay()
    {
        try
        {
            if (ToAdd)
                return;

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

    public override void OnPageOpen()
    {
        UpdatePrice();
        ToAdd = Parent.ToAdd;
        OnPropertyChanged(nameof(ToAdd));
    }


    #endregion
}
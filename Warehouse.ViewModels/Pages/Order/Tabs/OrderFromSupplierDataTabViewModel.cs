using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Warehouse.Core.Interface;
using Warehouse.Models.Enums;
using Warehouse.Models;
using Warehouse.ViewModel.Service;
using Warehouse.Core.Helpers;

namespace Warehouse.ViewModel.Pages;

public class OrderFromSupplierDataTabViewModel : BasePageViewModel
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

    public bool IsPrepared => _service.IsPrepared(_order.ID);

    public bool IsReceived => _service.IsReceived(_order.ID);

    public bool CanPrepared => !IsPrepared && !IsReceived && !ToAdd && (State != EOrderState.Finish);

    public bool CanReceived => IsPrepared && !IsReceived && !ToAdd && (State != EOrderState.Finish);

    #endregion

    #region Commands

    public ICommand SetStateCommand { get; protected set; }
    public ICommand SaveCommand { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public OrderFromSupplierDataTabViewModel(OrderEditAddPageViewModel parent , IApp app): base(app, parent)
    {
        Title = Core.Properties.Resources.Data;
        SetStateCommand = new RelayCommand<EOrderState>(SetState, (o) => State < o);
        SaveCommand = new RelayCommand(() => Parent?.SaveAdded(), () => ToAdd);
    }

    #endregion

    #region Command methods

    private void SetState(EOrderState obj)
    {
        try
        {
            if (_service == null)
                return;

            if (State >= obj)
                return;

            IWareHouseService whSer = Application.GetService<IWareHouseService>();
            if (obj == EOrderState.DeliveryPrepared)
            {
                if (!_service.SetAsPrepared(_order, whSer))
                    Application.ShowSilentMessage(Core.Properties.Resources.FailedToPrepared);
                else
                {
                    _service.Save();
                    Application.ShowSilentMessage(Core.Properties.Resources.SuccesfullPrepared, EMessageType.Ok);
                }
            }

            if (obj == EOrderState.Received)
            {
                if (!_service.SetAsReceived(_order, whSer))
                    Application.ShowSilentMessage(Core.Properties.Resources.FailedToPrepared);
                else
                {
                    _service.Save();
                    Application.ShowSilentMessage(Core.Properties.Resources.SuccesfullPrepared, EMessageType.Ok);
                }
            }
            OnPropertyChanged(nameof(CanReceived), nameof(CanPrepared));

        }
        catch (Exception ex) { Application.CatchExeption(ex); }
    }

    public void UpdatePrice()
    {
        try
        {
            if (_service != null)
                TotalPrice = _service.TotalPrice(_order);
        }
        catch (Exception ex) { Application.CatchExeption(ex); }
    }

    public override void OnPageOpen()
    {
        UpdatePrice();
        ToAdd = Parent.ToAdd;
        OnPropertyChanged(nameof(ToAdd), nameof(CanReceived), nameof(CanPrepared));
    }

    #endregion
}
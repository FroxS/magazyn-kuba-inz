using Warehouse.Models;
using System.ComponentModel.DataAnnotations;
using Warehouse.Core.Interface;
using Warehouse.Service.Interface;

namespace Warehouse.InnerDialog;

public class AddSupplierInnerDialogViewModel : BaseInnerDialogViewModel<Supplier>
{
    #region Private properties

    public string? _name;

    public uint _lp = 0;

    private readonly ISupplierService _service;

    #endregion

    #region Public properties

    [Required(ErrorMessage = "Name is required.")]
    public string? Name
    {
        get => _name;
        set
        {
            if (_name == value)
                return;
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public uint Lp
    {
        get => _lp;
        set
        {
            if (_lp == value)
                return;
            _lp = value;
            OnPropertyChanged(nameof(Lp));
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public AddSupplierInnerDialogViewModel(IApp app, ISupplierService service) : base(app)
    {
        _service = service;
        Result = null;
    }

    #endregion

    #region Public Methods

    protected override void Submit()
    {
        Result = null;
        string message = null;
        _CanValidate = true;
        message = GettErrors(nameof(Name));
        if (!string.IsNullOrWhiteSpace(message))
        {
            OnPropertyChanged(nameof(Name));
            return;
        }
            

        message = GettErrors(nameof(Lp));
        if (!string.IsNullOrWhiteSpace(message))
            return;

        var taks = _service.GetAll();
        if (taks.Find(o => o.Name == Name) != null)
        {
            CustomMessage.Add(nameof(Name), $"Nazwa {Name} juz istnieje w bazie danych");
            OnPropertyChanged(nameof(Name));
            return;
        }

        Result = Supplier.Get();
        Result.Name = Name;
        Result.Lp = Lp;
        base.Submit();

    }

    #endregion

}
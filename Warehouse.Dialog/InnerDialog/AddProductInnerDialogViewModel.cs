using Warehouse.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;

namespace Warehouse.InnerDialog;

public class AddProductInnerDialogViewModel : BaseInnerDialogViewModel<Product>
{
    #region Private properties

    public string? _name;
    public string? _description;
    public double _price;
    public ProductGroup? _group;
    public ProductStatus? _status;
    public Supplier? _supplier;
    public uint _lp = 0;
    private readonly IProductService _service;

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

    [Required(ErrorMessage = "Description is required.")]
    public string? Description
    {
        get => _description;
        set
        {
            if (_description == value)
                return;
            _description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    [Required(ErrorMessage = "Price is required.")]
    public double Price
    {
        get => _price;
        set
        {
            if (_price == value)
                return;
            _price = value;
            OnPropertyChanged(nameof(Price));
        }
    }

    [Required(ErrorMessage = "Product group is required.")]
    public ProductGroup? Group
    {
        get => _group;
        set
        {
            if (_group == value)
                return;
            _group = value;
            OnPropertyChanged(nameof(Group));
        }
    }

    [Required(ErrorMessage = "Product status is required.")]
    public ProductStatus? Status
    {
        get => _status;
        set
        {
            if (_status == value)
                return;
            _status = value;
            OnPropertyChanged(nameof(Status));
        }
    }

    [Required(ErrorMessage = "Supplier is required.")]
    public Supplier? Supplier
    {
        get => _supplier;
        set
        {
            if (_supplier == value)
                return;
            _supplier = value;
            OnPropertyChanged(nameof(Supplier));
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

    public ObservableCollection<ProductGroup> ProductGroups { get; }

    public ObservableCollection<Supplier> Suppliers { get; }

    public ObservableCollection<ProductStatus> ProductStatuses { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public AddProductInnerDialogViewModel(
        IApp app, 
        IProductService service,
        List<ProductGroup> productGroups,
        List<Supplier> supplierService,
        List<ProductStatus> productStatusService) 
        : base(app)
    {
        ProductGroups = new ObservableCollection<ProductGroup>(productGroups);
        Suppliers = new ObservableCollection<Supplier>(supplierService);
        ProductStatuses = new ObservableCollection<ProductStatus>(productStatusService);
        _service = service;
        Result = null;
    }

    #endregion

    #region Private Methods

    protected string[] GetpropsNameToFireOnSave()
    {
        return new string[] {
            nameof(Name),
            nameof(Description),
            nameof(Price),
            nameof(Group),
            nameof(Status),
            nameof(Supplier),
            nameof(Lp),
        };
    }
    protected override void Submit()
    {
        Result = null;
        string? message = null;
        _CanValidate = true;
        string[] props = GetpropsNameToFireOnSave();

        foreach (string prop in props)
        {
            message = GettErrors(prop);
            if (!string.IsNullOrWhiteSpace(message))
            {
                OnPropertyChanged(prop);
                return;
            }
        }

        var taks = _service.GetAll();
        if (taks.Find(o => o.Name == Name) != null)
        {
            CustomMessage.Add(nameof(Name), $"Nazwa {Name} juz istnieje w bazie danych");
            OnPropertyChanged(nameof(Name));
            return;
        }

        Result = Product.Get();
        Result.Name = Name;
        Result.Description = Description;
        Result.Price = Price;
        Result.ID_Status = Status.ID;
        Result.ID_Group = Group.ID;
        Result.ID_Supplier = Supplier.ID;
        Result.Lp = Lp;
        base.Submit();

    }

    #endregion

}
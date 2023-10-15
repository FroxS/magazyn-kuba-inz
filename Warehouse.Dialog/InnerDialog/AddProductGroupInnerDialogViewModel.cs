using Warehouse.Core.Interface;
using Warehouse.Models;
using System.ComponentModel.DataAnnotations;
using Warehouse.Service.Interface;

namespace Warehouse.InnerDialog;

public class AddProductGroupInnerDialogViewModel : BaseInnerDialogViewModel<ProductGroup>
{
    #region Private properties

    public string? _name;
    public string? _description;
    public uint _lp = 0;
    private readonly IProductGroupService _service;

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
    public AddProductGroupInnerDialogViewModel(
        IApp app, 
        IProductGroupService service )
        : base(app)
    {
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

        Result = ProductGroup.Get();
        Result.Name = Name;
        Result.Description = Description;
        Result.Lp = Lp;
        base.Submit();

    }

    #endregion

}
using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.ComponentModel.DataAnnotations;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class SupplierViewModel : BaseEntityViewModel<Supplier>
{
    #region Public Properties

    [Required(ErrorMessage = "Name is required.")]
    public string? Name
    {
        get => _entity.Name;
        set
        {
            SetProperty(() => Name, value, 
                () =>
                { 
                    if(_entity.Name != value)
                        Saved = false;
                    _entity.Name = value;
                }
            );
        }
    }

    #endregion

    #region Constructors
    public SupplierViewModel(ISupplierService service, Supplier product, IApp app) : base(service, product, app)
    {
        Saved = true;
    }

    #endregion

    #region Private methods

    public override string? Valid()
    {
        string message = null;

        message = GettErrors(nameof(Name));
        if (!string.IsNullOrWhiteSpace(message))
            return message;

        var taks = _service.GetAll();
        if (taks.Find(o => o.Name == Name && ID != o.ID) != null)
            return $"Nazwa {Name} juz istnieje w bazie danych";

        return base.Valid();
    }

    #endregion

    #region Command Methods


    #endregion

}

using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.ComponentModel.DataAnnotations;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class ProductStatusViewModel : BaseEntityViewModel<ProductStatus>
{
    #region Public Properties

    [Required(ErrorMessage = "Name is required.")]
    public string? Name
    {   
        get => _entity.Name;
        set
        {
            if (_entity.Name == value)
                return;
            Saved = false;
            _entity.Name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    #endregion

    #region Constructors
    public ProductStatusViewModel(IBaseService<ProductStatus> service, ProductStatus status):base(service, status)
    {
        _CanValidate = true;
        Saved = true;
    }

    #endregion

    #region Public methods

    public override string? Valid()
    {
        message = GettErrors(nameof(Name));
        if (!string.IsNullOrWhiteSpace(message))
            return message;

        var taks = _service.GetAll();
        if (taks.Find(o => o.Name == Name && ID != o.ID) != null)
            return $"Nazwa {Name} juz istnieje w bazie danych";

        return base.Valid();
    }

    #endregion

}

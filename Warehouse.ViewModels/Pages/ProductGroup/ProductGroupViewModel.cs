using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.ComponentModel.DataAnnotations;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class ProductGroupViewModel : BaseEntityViewModel<ProductGroup>
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


    [Required(ErrorMessage = "Description is required.")]
    public string? Description
    {
        get => _entity.Description;
        set
        {
            if (_entity.Description == value)
                return;
            Saved = false;
            _entity.Description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    #endregion

    #region Constructors
    public ProductGroupViewModel(IBaseService<ProductGroup> service, ProductGroup product, IApp app) : base(service, product, app)
    {
        _CanValidate = true;
        Saved = true;
    }

    #endregion

    #region Public methods

    public override string? Valid()
    {
        string message = null;
        message = GettErrors(nameof(Description));
        if (!string.IsNullOrWhiteSpace(message))
            return message;

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

using magazyn_kuba_inz.Core.Service;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.WareHouse;
using System.ComponentModel.DataAnnotations;

namespace magazyn_kuba_inz.Core.ViewModel.Pages;

public class SupplierViewModel : BaseEntityViewModel<Supplier>
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
    public SupplierViewModel(ISupplierService service, Supplier product) : base(service, product)
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

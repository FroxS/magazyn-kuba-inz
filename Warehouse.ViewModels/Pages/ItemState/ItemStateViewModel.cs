using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.ComponentModel.DataAnnotations;
using Warehouse.Service.Interface;
using Warehouse.Models.Enums;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class ItemStateViewModel : BaseEntityViewModel<ItemState>
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

    [Required(ErrorMessage = "State is required.")]
    public EState State
    {
        get => _entity.State;
        set
        {
            if (_entity.State == value)
                return;
            Saved = false;
            _entity.State = value;
            OnPropertyChanged(nameof(State));
        }
    }


    #endregion

    #region Constructors
    public ItemStateViewModel(IItemStateService service, ItemState product, IApp app) : base(service, product, app)
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

}

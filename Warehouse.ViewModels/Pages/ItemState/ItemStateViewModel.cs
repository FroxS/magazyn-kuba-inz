using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.ComponentModel.DataAnnotations;
using Warehouse.Service.Interface;

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


    public bool InWareHouse
    {
        get => _entity.InWareHouse;
        set
        {
            if (_entity.InWareHouse == value)
                return;
            Saved = false;
            _entity.InWareHouse = value;
            OnPropertyChanged(nameof(InWareHouse));
        }
    }

    public bool CanRealizeOrder
    {
        get => _entity.CanRealizeOrder;
        set
        {
            if (_entity.CanRealizeOrder == value)
                return;
            Saved = false;
            _entity.CanRealizeOrder = value;
            OnPropertyChanged(nameof(CanRealizeOrder));
        }
    }


    #endregion

    #region Constructors
    public ItemStateViewModel(IItemStateService service, ItemState product) : base(service, product)
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

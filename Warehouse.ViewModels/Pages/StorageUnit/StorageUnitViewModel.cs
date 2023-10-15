using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.ComponentModel.DataAnnotations;
using Warehouse.Service.Interface;

namespace Warehouse.ViewModel.Pages;

public class StorageUnitViewModel : BaseEntityViewModel<StorageUnit>
{
    #region Public Properties

    [Required(ErrorMessage = "Name is required.")]
    public string? Name
    {
        get => _entity.Name;
        set { SetProperty(() => Name, value, () => { Saved = _entity.Name == value; _entity.Name = value; } ); }
    }

    public double MaxWeight
    {
        get => _entity.MaxWeight;
        set { SetProperty(() => MaxWeight, value, () => { Saved = _entity.MaxWeight == value; _entity.MaxWeight = value; }); }
    }

    public double MaxHeight
    {
        get => _entity.MaxHeight;
        set { SetProperty(() => MaxHeight, value, () => { Saved = _entity.MaxHeight == value; _entity.MaxHeight = value; }); }
    }

    public double MaxWidth
    {
        get => _entity.MaxWidth;
        set { SetProperty(() => MaxWidth, value, () => { Saved = _entity.MaxWidth == value; _entity.MaxWidth = value; }); }
    }

    public double MaxDepth
    {
        get => _entity.MaxDepth;
        set { SetProperty(() => MaxDepth, value, () => { Saved = _entity.MaxDepth == value; _entity.MaxDepth = value; }); }
    }

    #endregion

    #region Constructors
    public StorageUnitViewModel(IStorageUnitService service, StorageUnit product) : base(service, product)
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

        return base.Valid();
    }

    #endregion

}

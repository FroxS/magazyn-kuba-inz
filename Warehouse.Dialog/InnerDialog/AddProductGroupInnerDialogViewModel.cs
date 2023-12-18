using Warehouse.Core.Interface;
using Warehouse.Models;
using System.ComponentModel.DataAnnotations;
using Warehouse.Service.Interface;
using Warehouse.ViewModel.Pages;

namespace Warehouse.InnerDialog;


public class AddProductGroupInnerDialogViewModel : BaseInnerDialogViewModel<ProductGroup>
{
    #region Private properties

    public ProductGroupViewModel _item;

    #endregion

    #region Public properties

    public ProductGroupViewModel Item
    {
        get => _item;
        set=> SetProperty(ref _item, value);
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public AddProductGroupInnerDialogViewModel(
        IApp app,
        IProductGroupService service)
        : base(app)
    {
        Item = new ProductGroupViewModel(service, ProductGroup.Get(), app);
        
        Result = null;
    }

    #endregion

    #region Private Methods

    protected override void Submit()
    {
        Result = null;
        Message.Clear();
        string? message = Item.Valid();
        _CanValidate = true;

        if (message != null)
        {
            Message.AddMessage(message);
            return;
        }

        Result = Item.Get();
        base.Submit();
    }

    #endregion
}
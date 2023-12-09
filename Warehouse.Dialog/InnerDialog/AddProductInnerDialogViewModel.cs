using Warehouse.Models;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Pages;

namespace Warehouse.InnerDialog;

public class AddProductInnerDialogViewModel : BaseInnerDialogViewModel<Product>
{
	#region Private properties

	public ProductViewModel _item;

	#endregion

	#region Public properties

	public ProductViewModel Item
	{
		get => _item;
		set => SetProperty(ref _item, value);
	}

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
		Item = new ProductViewModel(service, Product.Get(), productGroups,supplierService, productStatusService, app);
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
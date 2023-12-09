using Warehouse.Models;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Pages;

namespace Warehouse.InnerDialog;

public class AddProductStatusInnerDialogViewModel : BaseInnerDialogViewModel<ProductStatus>
{
	#region Private properties

	public ProductStatusViewModel _item;

	#endregion

	#region Public properties

	public ProductStatusViewModel Item
	{
		get => _item;
		set => SetProperty(ref _item, value);
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Default constructor
	/// </summary>
	public AddProductStatusInnerDialogViewModel(
        IApp app,
        IProductStatusService service )
        : base(app)
    {
		Item = new ProductStatusViewModel(service, ProductStatus.Get(),app);

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
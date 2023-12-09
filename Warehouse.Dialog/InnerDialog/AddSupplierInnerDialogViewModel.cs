using Warehouse.Models;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Pages;

namespace Warehouse.InnerDialog;

public class AddSupplierInnerDialogViewModel : BaseInnerDialogViewModel<Supplier>
{
	#region Private properties

	public SupplierViewModel _item;

	#endregion

	#region Public properties

	public SupplierViewModel Item
	{
		get => _item;
		set => SetProperty(ref _item, value);
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Default constructor
	/// </summary>
	public AddSupplierInnerDialogViewModel(IApp app, ISupplierService service) : base(app)
    {
		Item = new SupplierViewModel(service, Supplier.Get(), app);
		Result = null;
    }

	#endregion

	#region Public Methods

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
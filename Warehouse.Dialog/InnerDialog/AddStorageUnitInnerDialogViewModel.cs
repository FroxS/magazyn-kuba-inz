using Warehouse.Core.Interface;
using Warehouse.Models;
using Warehouse.ViewModel.Pages;

namespace Warehouse.InnerDialog;

public class AddStorageUnitInnerDialogViewModel : BaseInnerDialogViewModel<StorageUnit>
{
	#region Private properties

	public StorageUnitViewModel _item;

	#endregion

	#region Public properties

	public StorageUnitViewModel Item
	{
		get => _item;
		set => SetProperty(ref _item, value);
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Default constructor
	/// </summary>
	public AddStorageUnitInnerDialogViewModel(IApp app, IStorageUnitService service) : base(app)
    {
		Item = new StorageUnitViewModel(service, StorageUnit.Get(), app);
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
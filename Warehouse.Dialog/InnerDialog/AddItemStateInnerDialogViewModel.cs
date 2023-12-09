using Warehouse.Models;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Pages;

namespace Warehouse.InnerDialog;
public class AddItemStateInnerDialogViewModel : BaseInnerDialogViewModel<ItemState>
{
	#region Private properties

	public ItemStateViewModel _item;

	#endregion

	#region Public properties

	public ItemStateViewModel Item
	{
		get => _item;
		set => SetProperty(ref _item, value);
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Default constructor
	/// </summary>
	public AddItemStateInnerDialogViewModel(IApp app, IItemStateService service) : base(app)
    {
		Item = new ItemStateViewModel(service, ItemState.Get(), app);
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
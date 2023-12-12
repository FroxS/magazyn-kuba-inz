using Warehouse.Models;
using Warehouse.Core.Interface;
using System.Collections.ObjectModel;
using Warehouse.Core.Helpers;
using Warehouse.Models.Enums;

namespace Warehouse.ViewModel.Pages
{
    public class ProtuctStateWithOrderTabViewModel : ProtuctStateTabViewModel
    {
        #region Private properties

        #endregion

        #region Public properties

        
        #endregion

        #region Commands


        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProtuctStateWithOrderTabViewModel(IApp app, ItemState state) : base(app,state)
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProtuctStateWithOrderTabViewModel(ItemState state) : base(state)
        {

        }

		#endregion

		#region Command methods

		protected override void MoveToState(object[] elements)
		{
			try
			{
				IEnumerable<StorageItem>? items = (elements[1] as System.Collections.IList)?.Cast<StorageItem>();
				if (!(elements[0] is EState state))
					return;

				if (items == null)
					return;
				EState actualState = _state.State;

				if (actualState == state)
					return;

				ItemState targetState = _stateService.GetByState(state);

				if (targetState == null)
					return;

				string? message = null;

				foreach(var order in items.GroupBy(x => x.OrderItem?.Order))
				{
					if (order.Key == null)
						continue;

					StorageItem[] itemsInOrder = Items.Where(x => x.OrderItem?.Order.ID == order.Key.ID).ToArray();

					message = _service.MoveProductToState(targetState.State, itemsInOrder);
					if (message != null)
						break;
					if (message != null && _service.Save())
						_app.ShowSilentMessage(message ?? "Bład podczas zapisu");
				}
				OnPageOpen();
			}
			catch (Exception ex)
			{
				_app.CatchExeption(ex);
			}
		}

		#endregion

		#region Public methods

		#region Public methods

		public override void OnPageOpen()
		{
			if (_state == null)
				throw new ArgumentException("State is null");

            List<ItemState> allStatuses = _stateService.GetAll().ToList();
            StatusesToMove = new ObservableCollection<ItemState>();
            foreach(ItemState state in allStatuses)
            {
                if (state.State == _state.State)
                    continue;

                if (((int)_state.State).IsNeighborPowerTwo((int)state.State , true))
                {
                    StatusesToMove.Add(state);
				}

				if(state.State == EState.InStock && !StatusesToMove.Contains(state))
				{
					StatusesToMove.Add(state);
				}
            }
			Items = new ObservableCollection<StorageItem>(_service.GetProductsByStateWithOrders(_state.State));
            DefaultGroupBy = new ObservableCollection<string>() { 
				$"{nameof(StorageItem.OrderItem)}.{nameof(StorageItem.OrderItem.Order)}.{nameof(StorageItem.OrderItem.Order.Name)}" };
			int count = Items.Count();
			Title = $"{_state.Name} ({count})";
			SelectedItem = null;
		}

		#endregion


		#endregion
	}
}
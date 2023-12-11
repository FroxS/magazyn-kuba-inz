using Warehouse.Models;
using Warehouse.Core.Interface;
using System.Collections.ObjectModel;

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


		#endregion

		#region Public methods

		#region Public methods

		public override void OnPageOpen()
		{
			if (_state == null)
				throw new ArgumentException("State is null");

			Items = new ObservableCollection<StorageItem>(_service.GetProductsByStateWithOrders(_state.State));

            DefaultGroupBy = new ObservableCollection<string>() { "OrderItem.Order.Name" };

			int count = Items.Count();
			Title = $"{_state.Name} ({count})";
			SelectedItem = null;
		}

		#endregion


		#endregion
	}
}
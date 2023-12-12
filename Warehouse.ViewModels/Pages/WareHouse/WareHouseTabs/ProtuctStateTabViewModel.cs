using Warehouse.Core.Helpers;
using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Warehouse.Models.Enums;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages
{
    public class ProtuctStateTabViewModel : Tab
    {
        #region Private properties

        protected IWareHouseService _service => _app.GetService<IWareHouseService>();
        protected IItemStateService _stateService => _app.GetService<IItemStateService>();
        protected readonly IApp _app;
        protected readonly ItemState _state;
        protected ObservableCollection<ItemState> _statusesToMove;
        protected ObservableCollection<StorageItem> _items;
        protected StorageItem _selectedItem;
        protected bool _canMove = true;
        protected bool _canAddNew = true;
        protected bool _canChangeCount = true;
        protected string? _searchString;
        protected ObservableCollection<string> _defaultGroupBy = new ObservableCollection<string>();

		#endregion

		#region Public properties

		public ObservableCollection<StorageItem> Items
        {
            get => _items;
            protected set { SetProperty(ref _items, value); }
        }

        public ObservableCollection<ItemState> StatusesToMove
        {
            get => _statusesToMove;
            protected set { SetProperty(ref _statusesToMove, value); }
        }

        public StorageItem SelectedItem
        {
            get => _selectedItem;
            set { SetProperty(ref _selectedItem, value); }
        }

        public bool CanMove
        {
            get => _canMove;
            set { SetProperty(ref _canMove, value); }
        }

        public bool CanAddNew
        {
            get => _canAddNew;
            set { SetProperty(ref _canAddNew, value); }
        }

        public string? SearchString
        {
            get => _searchString;
            set { SetProperty(ref _searchString, value); }
        }

		public ObservableCollection<string> DefaultGroupBy
		{
			get => _defaultGroupBy;
			set { SetProperty(ref _defaultGroupBy, value); }
		}

		#endregion

		#region Commands

		public ICommand MoveToStateCommand { get; protected set; }

        public ICommand AddNewCommand { get; protected set; }

        public ICommand ChangeCountCommand { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProtuctStateTabViewModel(IApp app, ItemState state)
        {
            _app = app;
            Title = state?.Name ?? "";
            StatusesToMove = new ObservableCollection<ItemState>(_stateService.GetAll());
            StatusesToMove.Remove(state);
            _state = state;
            MoveToStateCommand = new RelayCommand<object[]>(MoveToState, (o) => SelectedItem != null && CanMove);
            AddNewCommand = new AsyncRelayCommand(AddItem, (o) => CanAddNew);
            CanAddNew = _service.CanAddProduct(Guid.Empty, state.State);
            OnPageOpen();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProtuctStateTabViewModel(ItemState state)
        {
            Title = state?.Name ?? "";
            _state = state;
        }

        #endregion

        #region Command methods

        protected virtual async Task AddItem()
        {
            if (!CanAddNew)
                return;
            Product p = _app.GetDialogService().GetProduct();
            if (p == null)
                return;
            double? count = await _app.GetInnerDialogService().GetCountAsync();

            if (count == null)
                return;

            if(!_service.Add(p.ID,_state.State, (int)count) && _service.Save())
                _app.ShowSilentMessage($"{Core.Properties.Resources.FailedToAddProduct}", EMessageType.Error);
            OnPageOpen();
        }

        protected virtual void MoveToState(object[] elements)
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

				foreach (var item in items)
				{
					message = _service.MoveProductToState(targetState.State, item);
					if (message != null)
						break;
					item.State = targetState;
					item.ID_State = targetState.ID;
				}
				if (message == null)
					_service.Save();
				else
					_app.ShowSilentMessage(message);
				OnPageOpen();
			}
			catch (Exception ex)
            {
                _app.CatchExeption(ex);
            }
            
        }

        #endregion

        #region Public methods

        public override void OnPageOpen()
        {
            if (_state == null)
                throw new ArgumentException("State is null");

            if(_service != null)
                Items = new ObservableCollection<StorageItem>(_service.GetProductsByState(_state.State));

			int count = Items?.Count() ?? 0;
            Title = $"{_state.Name} ({count})";
            SelectedItem = null;
        }

        #endregion
    }
}
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

        private readonly IWareHouseService _service;
        private readonly IItemStateService _stateService;
        private readonly IApp _app;
        private readonly ItemState _state;
        private ObservableCollection<ItemState> _statusesToMove;
        private ObservableCollection<StorageItem> _items;
        private StorageItem _selectedItem;
        public bool _canMove = true;
        public bool _canAddNew = true;
        public bool _canChangeCount = true;

        #endregion

        #region Public properties

        public ObservableCollection<StorageItem> Items
        {
            get => _items;
            private set { SetProperty(ref _items, value); }
        }

        public ObservableCollection<ItemState> StatusesToMove
        {
            get => _statusesToMove;
            private set { SetProperty(ref _statusesToMove, value); }
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

        #endregion

        #region Commands

        public ICommand MoveToStateCommand { get; private set; }

        public ICommand AddNewCommand { get; private set; }

        public ICommand ChangeCountCommand { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProtuctStateTabViewModel(IWareHouseService service, IItemStateService stateService, IApp app, ItemState state)
        {
            Title = state?.Name ?? "";
            _service = service;
            _stateService = stateService;
            _app = app;
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

        private async Task AddItem()
        {
            if (!CanAddNew)
                return;
            Product p = _app.GetDialogService().GetProduct();
            if (p == null)
                return;
            double? count = await _app.GetInnerDialogService().GetCountAsync();

            if (count == null)
                return;

            if(_service.Add(p.ID,_state.State, (int)count) && _service.Save())
                _app.ShowSilentMessage($"Udało się dodać produkt", EMessageType.Ok);
            else
                _app.ShowSilentMessage($"Nie udało sie dodac produktu", EMessageType.Error);
            OnPageOpen();
        }

        private void MoveToState(object[] elements)
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

            foreach(var item in items)
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

        #endregion

        #region Public methods

        public override void OnPageOpen()
        {
            if (_state == null)
                throw new ArgumentException("State is null");

            Items = new ObservableCollection<StorageItem>(_service.GetProductsByState(_state.State));
            int count = Items.Count();
            Title = $"{_state.Name} ({count})";
            SelectedItem = null;
        }

        #endregion
    }
}
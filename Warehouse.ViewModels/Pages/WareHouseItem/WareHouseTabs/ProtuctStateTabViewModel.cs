using Warehouse.Core.Helpers;
using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.Windows.Input;
using Warehouse.Service.Interface;
using System.Collections.ObjectModel;
using Warehouse.Models.Enums;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages
{
    public class ProtuctStateTabViewModel : Tab
    {
        #region Private properties

        private readonly IWareHouseItemService _service;
        private readonly IItemStateService _stateService;
        private readonly IApp _app;
        private readonly ItemState _state;
        private ObservableCollection<WareHouseItem> _items;
        private ObservableCollection<ItemState> _statusesToMove;
        private WareHouseItem _selectedItem;
        public bool _canMove = true;
        public bool _canAddNew = true;
        public bool _canChangeCount = true;

        #endregion

        #region Public properties

        public ObservableCollection<WareHouseItem> Items
        {
            get => _items;
            private set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public ObservableCollection<ItemState> StatusesToMove
        {
            get => _statusesToMove;
            private set
            {
                _statusesToMove = value;
                OnPropertyChanged(nameof(StatusesToMove));
            }
        }

        public WareHouseItem? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public bool CanChangeCount
        {
            get => _canChangeCount;
            set
            {
                _canChangeCount = value;
                OnPropertyChanged(nameof(CanChangeCount));
            }
        }

        public bool CanMove
        {
            get => _canMove;
            set
            {
                _canMove = value;
                OnPropertyChanged(nameof(CanMove));
            }
        }

        public bool CanAddNew
        {
            get => _canAddNew;
            set
            {
                _canAddNew = value;
                OnPropertyChanged(nameof(CanAddNew));
            }
        }

        #endregion

        #region Commands

        public ICommand MoveToStateCommand { get; private set; }

        public ICommand AddNewCommand { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProtuctStateTabViewModel(IWareHouseItemService service, IItemStateService stateService, IApp app, ItemState state)
        {
            Title = state.Name;
            _service = service;
            _stateService = stateService;
            _app = app;
            StatusesToMove = new ObservableCollection<ItemState>(_stateService.GetAll());
            StatusesToMove.Remove(state);
            _state = state;
            MoveToStateCommand = new AsyncRelayCommand<EState>(MoveToState, (o) => SelectedItem != null && CanMove);
            AddNewCommand = new AsyncRelayCommand(AddItem, (o) => CanAddNew);
            CanAddNew = ((state.State & (EState.Delivery | EState.InStock)) != 0);
            CanChangeCount = ((state.State & (EState.Delivery | EState.InStock)) != 0);
            Load();
        }

        #endregion

        #region Command methods

        private async Task AddItem()
        {
            if (!CanAddNew)
                return;
            Product p = _app.GetDialogService().GetProduct("Wybierz Produkt");
            if (p == null)
                return;
            double? count = await WareHouseApp.InnerDialog.GetCountAsync();

            if (count == null)
                return;
            WareHouseItem item = WareHouseItem.Get();
            item.ID_State = _state.ID;
            item.ID_Product = p.ID;
            item.Count = (int)count;

            if(_service.Add(item) && _service.Save())
            {
                _app.ShowSilentMessage($"Udało się dodać produkt", EMessageType.Ok);
            }
            else
            {
                _app.ShowSilentMessage($"Nie udało sie dodac produktu", EMessageType.Error);
            }
            Load();

        }

        private async Task MoveToState(EState state)
        {
            if (SelectedItem == null)
                return;
            EState actualState = _state.State;

            if (actualState == state)
                return;

            var itemState = _stateService.GetAll().FirstOrDefault(x => x.State == state);

            double? count = await WareHouseApp.InnerDialog.GetCountAsync();

            if (count != null && count > 0)
            {
                var item = SelectedItem;
                string message = _service.MoveProductToState(ref item, itemState.ID, (int)count);
                if (message == null)
                {
                    _service.Save();
                    
                }
                else
                {
                    _app.ShowSilentMessage(message);
                }
                Load();
            }
        }

        #endregion

        #region Public methods

        public override void Load()
        {
            if (_state == null)
                throw new ArgumentException("State is null");

            Items = new ObservableCollection<WareHouseItem>(_service.GetProductsByState(_state.ID));
            int count = Items.Select(x => x.Count).Sum();
            Title = $"{_state.Name} ({count})";
        }

        #endregion
    }
}
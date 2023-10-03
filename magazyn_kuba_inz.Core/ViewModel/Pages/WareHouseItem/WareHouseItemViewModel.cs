using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.WareHouse;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel.Pages
{
    public delegate void ItemUpdated(WareHouseItem newItem, WareHouseItem oldItem);

    public class WareHouseItemViewModel : BaseEntityViewModel<WareHouseItem>
    {
        #region Private properties

        protected IWareHouseItemService? _wareHouseService => _service as IWareHouseItemService;

        #endregion

        #region Public properties

        public Product? Product => _entity?.Product;

        public string? Name => Product?.Name;

        public string? Description => Product?.Description;

        public ItemState? State => _entity?.State;

        public int? Count => _entity?.Count;

        #endregion

        #region Events

        public event ItemUpdated OnItemCountChange;

        #endregion

        #region Commands

        public ICommand AddProductCommand { get; protected set; }

        public ICommand RemoveProductCommand { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public WareHouseItemViewModel(IWareHouseItemService service, WareHouseItem product) : base(service, product)
        {
            Saved = true;
            AddProductCommand = new AsyncRelayCommand(AddProduct);
            RemoveProductCommand = new AsyncRelayCommand(RemoveProduct);
        }

        #endregion

        #region Command methods

        protected override string[] GetpropsNameToFireOnSave()
        {
            return new string[] {
                nameof(Product),
                nameof(Name),
                nameof(Description),
                nameof(State),
                nameof(Count)
            };
        }

        protected virtual async Task AddProduct()
        {
            try
            {
                if (Product != null && State != null && _wareHouseService != null)
                {
                    var added = await _wareHouseService.AddProduct(Product, State);
                    if (added == null)
                        Message = "Nie udało się dodać";
                    else
                        _entity = added;
                    OnPropertiesChanged(GetpropsNameToFireOnSave());
                }
            }
            catch (Exception ex) { }
            
        }

        protected virtual async Task RemoveProduct()
        {
            try
            {
                if (Product != null && State != null && _wareHouseService != null)
                {
                    var added = await _wareHouseService.RemoveProduct(Product, State);
                    OnItemCountChange?.Invoke(added, _entity);
                    _entity = added;
                    OnPropertiesChanged(GetpropsNameToFireOnSave());
                }
            }
            catch (Exception ex) { }

        }

        #endregion

    }
}
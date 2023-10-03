using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Pages;
using magazyn_kuba_inz.Models.WareHouse;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace magazyn_kuba_inz.Core.ViewModel.InnerDialog
{
    public class AddProductToStateInnerDialogViewModel : BaseInnerDialogViewModel<WareHouseItem>
    {
        #region Private properties

        private readonly Product _product;
        private ItemState? state;
        private int _count = 1;

        #endregion

        #region Public properties

        public string? Name
        {
            get => _product.Name;
        }

        public string? Description
        {
            get => _product.Description;
        }

        [Required(ErrorMessage = "State is required.")]
        public ItemState? State
        {
            get => state;
            set
            {
                state = value;
                OnPropertyChanged(nameof(State));
            }
        }

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        public ObservableCollection<ItemState> LeftStates { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AddProductToStateInnerDialogViewModel(
            IApp app,
            Product product,
            List<ItemState> leftStates)
            : base(app)
        {
            _product = product;
            LeftStates = new ObservableCollection<ItemState>(leftStates);
            Result = null;
        }

        #endregion

        #region Private Methods

        protected string[] GetpropsNameToFireOnSave()
        {
            return new string[] {
                nameof(State)
            };
        }
        protected override void Submit()
        {
            Result = null;
            string? message = null;
            _CanValidate = true;
            string[] props = GetpropsNameToFireOnSave();

            foreach (string prop in props)
            {
                message = GettErrors(prop);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    OnPropertyChanged(prop);
                    return;
                }
            }

            Result = WareHouseItem.Get();
            //Result.Product = _product;
            //Result.State = State;
            Result.Count = Count;
            Result.ID_State = State.ID;
            Result.ID_Product = _product.ID;
            base.Submit();
        }

        #endregion

    }
}
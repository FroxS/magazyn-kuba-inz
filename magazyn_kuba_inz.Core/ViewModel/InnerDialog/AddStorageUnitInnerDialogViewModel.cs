using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Models.WareHouse;
using System.ComponentModel.DataAnnotations;

namespace magazyn_kuba_inz.Core.ViewModel.InnerDialog
{
    public class AddStorageUnitInnerDialogViewModel : BaseInnerDialogViewModel<StorageUnit>
    {
        #region Private properties

        private readonly IStorageUnitService _service;
        public string? _name;
        public uint _lp;
        public double _maxWeight;
        public double _maxWidth;
        public double _maxHeight;
        public double _maxDepth;

        #endregion

        #region Public properties

        [Required(ErrorMessage = "Name is required.")]
        public string? Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public uint Lp
        {
            get => _lp;
            set { _lp = value; OnPropertyChanged(nameof(Lp)); }
        }

        public double MaxWeight
        {
            get => _maxWeight;
            set { _maxWeight = value; OnPropertyChanged(nameof(MaxWeight)); }
        }

        public double MaxWidth
        {
            get => _maxWidth;
            set { _maxWidth = value; OnPropertyChanged(nameof(MaxWidth)); }
        }

        public double MaxHeight
        {
            get => _maxHeight;
            set { _maxHeight = value; OnPropertyChanged(nameof(MaxHeight)); }
        }

        public double MaxDepth
        {
            get => _maxDepth;
            set { _maxDepth = value; OnPropertyChanged(nameof(MaxDepth)); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AddStorageUnitInnerDialogViewModel(IApp app, IStorageUnitService service) : base(app)
        {
            _service = service;
            Result = null;
            Lp = 0;
        }

        #endregion

        #region Private Methods

        protected string[] GetpropsNameToFireOnSave()
        {
            return new string[] {
                nameof(Name),
                nameof(Lp),
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

            Result = new  StorageUnit();
            Result.Name = Name;
            Result.Lp = Lp;
            Result.MaxWeight = MaxWeight;
            Result.MaxWidth =  MaxWidth ;
            Result.MaxHeight = MaxHeight;
            Result.MaxDepth = MaxDepth;
            base.Submit();
        }

        #endregion

    }
}
using Warehouse.Models;
using System.Collections.ObjectModel;
using Warehouse.Service.Interface;

namespace Warehouse.Dialog
{
    internal class StorageUnitDialogViewModel : DialogViewModelBase<StorageUnit>
    {
        #region Private properties

        private ObservableCollection<StorageUnit> _units;

        private StorageUnit _unit;

        private string _search;

        #endregion

        #region Public properties

        public ObservableCollection<StorageUnit> Units
        {
            get => _units;
            set { _units = value; OnPropertyChanged(nameof(Units)); }
        }

        public StorageUnit Unit
        {
            get => _unit;
            set { _unit = value; OnPropertyChanged(nameof(Unit)); }
        }

        public string Search
        {
            get => _search;
            set { _search = value; OnPropertyChanged(nameof(Search)); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public StorageUnitDialogViewModel(IStorageUnitService service, string message) : base(message)
        {
            Units = new ObservableCollection<StorageUnit>(service.GetAll());
            Unit = Units.FirstOrDefault();
        }


        #endregion

        #region Command methods

        protected override void ok()
        {
            DialogResult = Unit;
            if (DialogResult == null)
                return;
            base.ok();
        }


        #endregion
    }
}
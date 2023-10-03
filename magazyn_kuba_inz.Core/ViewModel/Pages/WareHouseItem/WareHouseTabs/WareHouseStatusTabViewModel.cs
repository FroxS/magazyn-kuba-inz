using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.WareHouse;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel.Pages
{
    public class WareHouseStatusTabViewModel : WareHouseItemViewModel, ITab
    {
        #region Private properties

        #endregion

        #region Public properties

        public string Title => $"{(_entity?.State?.Name ?? "State ")} ({Count})";

        #endregion

        #region Commands

        public ICommand CloseTab { get; private set; }


        #endregion

        #region Events

        public event EventHandler CloseRequest;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public WareHouseStatusTabViewModel(IWareHouseItemService service, WareHouseItem product) : base(service, product)
        {
        }

        #endregion

        #region Command methods

        #endregion

        #region Public methods

        public void Load() { }

        #endregion
    }
}
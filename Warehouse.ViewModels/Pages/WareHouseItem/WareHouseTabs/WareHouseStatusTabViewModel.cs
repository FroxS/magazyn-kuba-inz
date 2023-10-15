using Warehouse.Core.Helpers;
using Warehouse.ViewModel.Service;
using Warehouse.Models;
using System.Windows.Input;
using Warehouse.Service.Interface;

namespace Warehouse.ViewModel.Pages
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
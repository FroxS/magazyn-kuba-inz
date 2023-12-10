using Warehouse.Models;
using Warehouse.Core.Interface;

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

        

        #endregion
    }
}
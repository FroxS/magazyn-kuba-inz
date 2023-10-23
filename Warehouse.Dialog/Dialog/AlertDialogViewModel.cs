using Warehouse.Core.Helpers;
using Warehouse.Models.Enums;
using Warehouse.Core.Interface;

namespace Warehouse.Dialog
{
    internal class AlertDialogViewModel : DialogViewModelBase<EDialogResult>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AlertDialogViewModel(string message, string title = "") : base(title, message)
        {
            OKCommand = new RelayCommand(Yes);
        }

        #endregion

        #region Command methods

        private void Yes()
        {
            CloseDialogWithResult(EDialogResult.Yes);
        }

        #endregion
    }
}
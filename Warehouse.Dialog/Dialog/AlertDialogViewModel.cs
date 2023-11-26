using Warehouse.Core.Helpers;
using Warehouse.Models.Enums;
using Warehouse.Core.Interface;
using System.Windows.Input;

namespace Warehouse.Dialog
{
    internal class AlertDialogViewModel : DialogViewModelBase<EDialogResult>
    {
        #region Commands

        public ICommand CoppyCommand { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AlertDialogViewModel(string message, string title = "") : base(title, message)
        {
            OKCommand = new RelayCommand(Yes);
            CoppyCommand = new RelayCommand(() => { System.Windows.Clipboard.SetText(Message); });
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
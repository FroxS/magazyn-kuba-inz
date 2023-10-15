using Warehouse.Core.Helpers;
using System.Windows.Input;
using Warehouse.Models.Enums;
using Warehouse.Core.Interface;

namespace Warehouse.Dialog
{
    public interface IAlertDialogView { }
    public class AlertDialogViewModel : DialogViewModelBase<EDialogResult>
    {
        #region Public properties

        public override ICommand OKCommand { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AlertDialogViewModel(string message, string title = "") : base(title, message)
        {
            OKCommand = new RelayCommand<IDialogWindow>(Yes);
        }

        #endregion

        #region Command methods

        private void Yes(IDialogWindow window)
        {
            CloseDialogWithResult(window, EDialogResult.Yes);
        }

        #endregion
    }
}
using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Service.Dialog;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel.Dialog
{
    public interface IAlertDialogView { }
    public class AlertDialogViewModel : DialogViewModelBase<DialogResult>
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
            CloseDialogWithResult(window, DialogResult.Yes);
        }

        #endregion
    }
}
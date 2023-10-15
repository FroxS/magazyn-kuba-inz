using Warehouse.Core.Helpers;
using System.Windows.Input;
using Warehouse.Models.Enums;
using Warehouse.Core.Interface;

namespace Warehouse.Dialog
{
    public interface IYesNoDialogView { }
    public class YesNoDialogViewModel: DialogViewModelBase<EDialogResult>
    {
        #region Private properties

        #endregion

        #region Public properties

        public override ICommand OKCommand { get; protected set; }

        public ICommand NoCommand { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public YesNoDialogViewModel(string title, string message) : base(title, message)
        {
            OKCommand = new RelayCommand<IDialogWindow>(Yes);
            NoCommand = new RelayCommand<IDialogWindow>(No);
        }


        #endregion

        #region Command methods

        private void Yes(IDialogWindow window)
        {
            CloseDialogWithResult(window, EDialogResult.Yes);
        }

        private void No(IDialogWindow window)
        {
            CloseDialogWithResult(window, EDialogResult.No);
        }

        #endregion
    }
}
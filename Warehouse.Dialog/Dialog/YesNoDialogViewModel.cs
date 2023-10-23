using Warehouse.Core.Helpers;
using System.Windows.Input;
using Warehouse.Models.Enums;
using Warehouse.Core.Interface;

namespace Warehouse.Dialog
{
    internal class YesNoDialogViewModel: DialogViewModelBase<EDialogResult>
    {
        #region Private properties

        #endregion

        #region Public properties


        public ICommand NoCommand { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public YesNoDialogViewModel(string title, string message) : base(title, message)
        {
            OKCommand = new RelayCommand(Yes);
            NoCommand = new RelayCommand(No);
        }

        #endregion

        #region Command methods

        private void Yes()
        {
            CloseDialogWithResult(EDialogResult.Yes);
        }

        private void No()
        {
            CloseDialogWithResult(EDialogResult.No);
        }

        #endregion
    }
}
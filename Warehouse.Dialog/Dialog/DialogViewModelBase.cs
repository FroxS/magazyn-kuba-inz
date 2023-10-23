using Warehouse.Core.Helpers;
using System.Windows.Input;
using Warehouse.Models;
using Warehouse.Core.Interface;
using System.Windows;

namespace Warehouse.Dialog
{
    internal abstract class DialogViewModelBase<T> : ObservableObject, IDialogViewModelBase<T>
    {

        #region Public properties

        public string Title { get; set; }
        public string Message { get; set; }
        public T DialogResult { get; set; }

        public Window Window { protected get; set; }

        #endregion

        #region Commands

        public ICommand OKCommand { get; protected set; }
        public ICommand ExitCommand { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public DialogViewModelBase(string title) : this(title, string.Empty) { }
        public DialogViewModelBase(string title, string message)
        {
            Title = title;
            Message = message;
            ExitCommand = new RelayCommand(exit);
            OKCommand = new RelayCommand(ok);
        }

        #endregion

        #region Private methods

        private void exit()
        {
            Window.DialogResult = false;
        }

        #endregion

        #region Public methods

        protected virtual void ok()
        {
            CloseDialogWithResult(DialogResult);
        }

        public void CloseDialogWithResult( T result)
        {
            DialogResult = result;
            if (Window != null)
            {
                Window.DialogResult = true;
            }
        }

        #endregion
    }
}
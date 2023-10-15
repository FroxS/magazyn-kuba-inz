using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Windows;
using Warehouse.Core.Interface;
using Warehouse.Models.Enums;

namespace Warehouse.Dialog
{
    public class DialogService : IDialogService
    {
        #region Private properties

        private readonly IServiceProvider _service;

        #endregion

        #region Public methods

        public T OpenDialog<T>(IDialogViewModelBase<T> wm)
        {
            IDialogWindow window = _service.GetRequiredService<IDialogWindow>();
            var control = GetControlDialog(wm);
            if (control == null)
                throw new ArgumentException("Dialog now found");
            window.Control = control;
            window.ShowDialog();
            return wm.DialogResult;

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public DialogService(IServiceProvider service)
        {
            _service = service;
        }

        #endregion

        #region Private methods

        public FrameworkElement? GetControlDialog<T>(IDialogViewModelBase<T> wm)
        {
            if(wm is AlertDialogViewModel)
                return _service.GetRequiredService<IAlertDialogView>() as FrameworkElement;
            if (wm is YesNoDialogViewModel)
                return _service.GetRequiredService<IYesNoDialogView>() as FrameworkElement;

            Debugger.Break();
            return null;
        }

        #endregion

        #region Public method

        public void ShowAlert(string message, string title = "") 
        {
            OpenDialog(new AlertDialogViewModel(message,title));
        }

        public EDialogResult GetYesNoDialog(string message, string title = "")
        {
            return OpenDialog(new YesNoDialogViewModel(message, title));
        }

        #endregion
    }
}
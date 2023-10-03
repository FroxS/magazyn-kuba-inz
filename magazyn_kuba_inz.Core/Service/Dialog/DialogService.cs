using magazyn_kuba_inz.Core.ViewModel.Dialog;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Windows;

namespace magazyn_kuba_inz.Core.Service.Dialog
{
    public class DialogService : IDialogService
    {
        #region Private properties

        private readonly IServiceProvider _service;

        #endregion

        #region Public methods

        public T OpenDialog<T>(DialogViewModelBase<T> wm)
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

        public FrameworkElement? GetControlDialog<T>(DialogViewModelBase<T> wm)
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

        public DialogResult GetYesNoDialog(string message, string title = "")
        {
            return OpenDialog(new YesNoDialogViewModel(message, title));
        }

        #endregion
    }
}
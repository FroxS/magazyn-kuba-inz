using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Windows;
using Warehouse.Core.Interface;
using Warehouse.Dialog.View;
using Warehouse.Models;
using Warehouse.Models.Enums;
using Warehouse.Service.Interface;

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
            var window = GetControlDialog(wm);

            if (window == null)
                throw new ArgumentException("Dialog now found");
            //window.Parent = IApp.MainWindow;
            try
            {
                window.Owner = _service.GetService<IApp>().MainWindow;
            }
            catch { }
            
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

        public Window? GetControlDialog<T>(IDialogViewModelBase<T> vm)
        {
            if(vm is AlertDialogViewModel avm)
                return new AlertDialog(avm);
            if (vm is YesNoDialogViewModel ynvm)
                return new YesNoDialog(ynvm);
            if (vm is ProductDialogViewModel pvm)
                return new ProductDialog(pvm);
            if (vm is StorageUnitDialogViewModel suvm)
                return new StorageUnitDialog(suvm);

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
            return OpenDialog(new YesNoDialogViewModel( title, message));
        }

        public Product GetProduct(string message)
        {
            return OpenDialog(new ProductDialogViewModel(_service.GetService<IProductService>() ,message));
        }

        public void ShowError(Exception ex)
        {
            OpenDialog(new AlertDialogViewModel(ex.Message, "Bład aplikacji"));
        }

        public StorageUnit GetStorageUnit(string message)
        {
            return OpenDialog(new StorageUnitDialogViewModel(_service.GetService<IStorageUnitService>(), message));
        }

        #endregion
    }
}
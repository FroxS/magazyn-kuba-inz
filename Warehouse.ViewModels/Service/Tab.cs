using Warehouse.Core.Helpers;
using System.Windows.Input;
using Warehouse.Models;

namespace Warehouse.ViewModel.Service
{
    public interface ITab
    {
        string Title { get; }
        ICommand CloseTab { get; }
        event EventHandler CloseRequest;
        void Load();
    }

    public abstract class Tab : ObservableObject, ITab
    {
        #region Public Properties

        public string Title { get; set; }

        public bool IsEmpty { get; set; } = true;

        #endregion

        #region Commands

        public ICommand CloseTab { get; set; }
        public ICommand OpenTabCommand { get; set; }

        #endregion

        #region Events

        public event EventHandler CloseRequest;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Tab()
        {
            CloseTab = new RelayCommand(p => CloseRequest?.Invoke(this, EventArgs.Empty));
        }

        #endregion

        #region Abstract methods

        public abstract void Load();

        #endregion
    }
}
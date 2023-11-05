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

    public abstract class Tab : BaseViewModel, ITab
    {
        #region Private fields

        private string _title;

        private bool _isEmpty = true;

        #endregion

        #region Public Properties

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public bool IsEmpty
        {
            get => _isEmpty;
            set { _isEmpty = value; OnPropertyChanged(nameof(IsEmpty)); }
        }

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
            CloseTab = new RelayCommand(() => CloseRequest?.Invoke(this, EventArgs.Empty));
        }

        #endregion

        #region Abstract methods

        public abstract void Load();

        #endregion
    }
}
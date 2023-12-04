using Warehouse.Core.Helpers;
using System.Windows.Input;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Service
{
    public abstract class Tab : BaseViewModel, ITab
    {
        #region Private fields

        private string _title;

        private bool _isEmpty = true;

        #endregion

        #region Public Properties

        public virtual string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool IsEmpty
        {
            get => _isEmpty;
            protected set => SetProperty(ref _isEmpty, value);
        }

        public virtual BaseViewModel Parent { get;}

        #endregion

        #region Commands

        public ICommand CloseTabCommand { get; protected set; }
        public ICommand OpenTabCommand { get; protected set; }

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
            CloseTabCommand = new RelayCommand(
                () => 
                CloseRequest?.Invoke(this, EventArgs.Empty)
                );
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Tab(BaseViewModel parent) : this()
        {
            Parent = parent;
        }

        #endregion

        #region Abstract methods

        public virtual void OnPageOpen() { }

        public virtual void OnPageClose() { }

        #endregion
    }
}
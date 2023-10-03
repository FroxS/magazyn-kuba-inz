using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel.InnerDialog
{
    public abstract class BaseInnerDialogViewModel<T> : BaseViewModel
    {
        #region Private properties

        protected readonly IApp _app;

        #endregion

        #region Public properties

        public T? Result { get; set; }

        public bool DialogResult { get; private set; } = false;

        #endregion

        #region Commands

        public ICommand SubmitCommand { get; protected set; }

        public ICommand ExitCommand { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseInnerDialogViewModel(IApp app)
        {
            DialogResult = false;
            _app = app;
            SubmitCommand = new RelayCommand((o) => { Submit(); });
            ExitCommand = new RelayCommand((o) => { Exit(); });
        }

        #endregion

        #region Command Methods

        protected virtual void Submit()
        {
            _app.GetInnerDialogService().CloseInnerDialog();
            DialogResult = true;
        }

        protected virtual void Exit()
        {
            _app.GetInnerDialogService().CloseInnerDialog();
            DialogResult = true;
        }

        #endregion
    }
}
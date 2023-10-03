using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Service.Dialog;
using magazyn_kuba_inz.Core.Service.Interface;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel.InnerDialog
{
    public class YesNoInnerDialogViewModel : BaseInnerDialogViewModel<DialogResult>
    {
        #region Private fields

        private string _message;

        #endregion

        #region Public properties

        public string Message 
        { 
            get => _message;
            set { 
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        #endregion

        #region Commands

        public ICommand NoCommand { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public YesNoInnerDialogViewModel(string message, IApp app) : base(app)
        {
            Message = message;
            NoCommand = new RelayCommand((o) => No()); ;
            Result = Core.Service.Dialog.DialogResult.Undefind;
        }

        #endregion

        #region Command Methods

        protected override void Submit()
        {
            Result = Core.Service.Dialog.DialogResult.Yes;
            base.Submit();
        }

        private void No()
        {
            Result = Core.Service.Dialog.DialogResult.No;
            base.Submit();
        }

        #endregion
    }
}
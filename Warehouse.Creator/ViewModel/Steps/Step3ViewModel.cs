using Warehouse.Core.Interface;
using Warehouse.Creator.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.Creator.ViewModel
{
    public class Step3ViewModel : BaseStepViewModel
    {
        #region Private properties

        private SuppliersPageViewModel _groupVM;

        #endregion

        #region Public properties

        public SuppliersPageViewModel GroupVM
        {
            get => _groupVM;
            set => SetProperty(ref _groupVM, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Step3ViewModel(IApp app) : base(app)
        {
            Step = EPageStep.Step3;
        }

        #endregion

        #region Command methods

        protected override void NextStep()
        {
            try
            {
                GroupVM.OnPageClose();

                if (GroupVM.Items.Count < 2)
                {
                    Application.GetDialogService().ShowAlert("Dodaj minimum 2 dostawców");
                    return;
                }
                if (!GroupVM.CanChangePage)
                {
                    Application.GetDialogService().ShowAlert("Can't go next");
                    return;
                }
                this.SetNextStep();

            }catch(Exception ex)
            {
                Application.CatchExeption(ex);
            }
        }

        #endregion

        #region Helpers

        public override void OnPageOpen()
        {
            GroupVM = Application.GetService<SuppliersPageViewModel>();
            GroupVM.OnPageOpen();
        }

        #endregion
    }
}
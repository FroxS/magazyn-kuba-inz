using Warehouse.Core.Interface;
using Warehouse.Creator.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.Creator.ViewModel
{
    public class Step4ViewModel : BaseStepViewModel
    {
        #region Private properties

        private ProductStatusesPageViewModel _groupVM;

        #endregion

        #region Public properties

        public ProductStatusesPageViewModel GroupVM
        {
            get => _groupVM;
            set => SetProperty(ref _groupVM, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Step4ViewModel(IApp app) : base(app)
        {
            Step = EPageStep.Step4;
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
                    Application.GetDialogService().ShowAlert("Dodaj minimum 2 statusy");
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
            GroupVM = Application.GetService<ProductStatusesPageViewModel>();
            GroupVM.OnPageOpen();
        }

        #endregion
    }
}
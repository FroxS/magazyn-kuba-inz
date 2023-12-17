using Warehouse.Core.Interface;
using Warehouse.Creator.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.Creator.ViewModel
{
    public class Step2ViewModel : BaseStepViewModel
    {
        #region Private properties

        private ProductGroupsPageViewModel _groupVM;

        #endregion

        #region Public properties

        public ProductGroupsPageViewModel GroupVM
        {
            get => _groupVM;
            set => SetProperty(ref _groupVM, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Step2ViewModel(IApp app) : base(app)
        {
            Step = EPageStep.Step2;
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
                    Application.GetDialogService().ShowAlert("Dodaj minimu 2 grupy poduktow");
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
            GroupVM = Application.GetService<ProductGroupsPageViewModel>();
            GroupVM.OnPageOpen();
        }

        #endregion
    }
}
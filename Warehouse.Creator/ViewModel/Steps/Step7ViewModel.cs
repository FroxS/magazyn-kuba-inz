using Warehouse.Core.Interface;
using Warehouse.Creator.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.Creator.ViewModel
{
    public class Step7ViewModel : BaseStepViewModel
    {
        #region Private properties

        private WareHouseCreatorViewModel _groupVM;

        #endregion

        #region Public properties

        public WareHouseCreatorViewModel GroupVM
        {
            get => _groupVM;
            set => SetProperty(ref _groupVM, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Step7ViewModel(IApp app) : base(app)
        {
            Step = EPageStep.Step7;
        }

        #endregion

        #region Command methods

        protected override void NextStep()
        {
            try
            {
                GroupVM.OnPageClose();
                if (!GroupVM.CanChangePage)
                {
                    Application.GetDialogService().ShowAlert("Can't go next");
                    return;
                }

                Application.GetDialogService().ShowAlert("Koniec");
                

            }catch(Exception ex)
            {
                Application.CatchExeption(ex);
            }
        }

        #endregion

        #region Helpers

        public override void OnPageOpen()
        {
            GroupVM = new WareHouseCreatorViewModel(Application, this);
            GroupVM.OnPageOpen();
        }

        #endregion
    }
}
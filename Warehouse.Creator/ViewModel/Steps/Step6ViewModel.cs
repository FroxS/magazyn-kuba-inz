using Warehouse.Core.Interface;
using Warehouse.Creator.Service;
using Warehouse.Models;
using Warehouse.Models.Enums;
using Warehouse.ViewModel.Pages;

namespace Warehouse.Creator.ViewModel
{
    public class Step6ViewModel : BaseStepViewModel
    {
        #region Private properties

        private ItemStatesPageViewModel _groupVM;

        #endregion

        #region Public properties

        public ItemStatesPageViewModel GroupVM
        {
            get => _groupVM;
            set => SetProperty(ref _groupVM, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Step6ViewModel(IApp app) : base(app)
        {
            Step = EPageStep.Step6;
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
            IItemStateService serice = Application.GetService<IItemStateService>();
            List<ItemState> all = serice.GetAll();
            foreach (EState state in Enum.GetValues<EState>())
            {
                ItemState? found = all.FirstOrDefault(x => x.State == state);
                if(found == null)
                {
                    found = ItemState.Get();
                    found.State = state;
                    found.Name = state.ToString();
                    serice.Add(found);
                }
            }
            serice.Save();
            GroupVM = Application.GetService<ItemStatesPageViewModel>();
            GroupVM.OnPageOpen();
        }

        #endregion
    }
}
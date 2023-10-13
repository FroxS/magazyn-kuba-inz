using magazyn_kuba_inz.Core.Models;
using magazyn_kuba_inz.Core.Service.Interface;
using System.ComponentModel.DataAnnotations;

namespace magazyn_kuba_inz.Core.ViewModel.InnerDialog
{
    public class GetHallInnerDialogViewModel : BaseInnerDialogViewModel<HallObject>
    {
        #region Private properties

        public HallObject? _hall;

        #endregion

        #region Public properties

        [Required(ErrorMessage = "Name is required.")]
        public HallObject? Hall
        {
            get => _hall;
            set
            {
                _hall = value;
                OnPropertyChanged(nameof(Hall));
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public GetHallInnerDialogViewModel(IApp app, HallObject obj = null) : base(app)
        {
            Result = null;
            Hall = obj ?? new HallObject(Guid.NewGuid()) { Width = 1000, Height = 1000, Name = "Hala" };
        }

        #endregion

        #region Public Methods

        protected override void Submit()
        {
            Result = null;
            
            if(Hall?.Width <= 0 || Hall?.Height <= 0)
            {
                CustomMessage.Add(nameof(Hall), "Nieodpowiednia wysokość oraz szerokość hali");
            }

            Result = Hall;
            base.Submit();
        }

        #endregion

    }
}
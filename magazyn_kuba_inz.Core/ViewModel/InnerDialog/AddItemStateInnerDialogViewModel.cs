﻿using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Models.WareHouse;
using System.ComponentModel.DataAnnotations;

namespace magazyn_kuba_inz.Core.ViewModel.InnerDialog
{
    public class AddItemStateInnerDialogViewModel : BaseInnerDialogViewModel<ItemState>
    {
        #region Private properties

        public string? _name;
        public uint _lp = 0;
        private readonly IItemStateService _service;

        #endregion

        #region Public properties

        [Required(ErrorMessage = "Name is required.")]
        public string? Name
        {
            get => _name;
            set
            {
                if (_name == value)
                    return;
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public uint Lp
        {
            get => _lp;
            set
            {
                if (_lp == value)
                    return;
                _lp = value;
                OnPropertyChanged(nameof(Lp));
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AddItemStateInnerDialogViewModel(IApp app, IItemStateService service) : base(app)
        {
            _service = service;
            Result = null;
        }

        #endregion

        #region Private Methods

        protected string[] GetpropsNameToFireOnSave()
        {
            return new string[] {
                nameof(Name),
                nameof(Lp),
            };
        }
        protected override void Submit()
        {
            Result = null;
            string? message = null;
            _CanValidate = true;
            string[] props = GetpropsNameToFireOnSave();

            foreach (string prop in props)
            {
                message = GettErrors(prop);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    OnPropertyChanged(prop);
                    return;
                }
            }

            var taks = _service.GetAll();
            if (taks.Find(o => o.Name == Name) != null)
            {
                CustomMessage.Add(nameof(Name), $"Nazwa {Name} juz istnieje w bazie danych");
                OnPropertyChanged(nameof(Name));
                return;
            }

            Result = ItemState.Get();
            Result.Name = Name;
            Result.Lp = Lp;
            base.Submit();

        }

        #endregion

    }
}
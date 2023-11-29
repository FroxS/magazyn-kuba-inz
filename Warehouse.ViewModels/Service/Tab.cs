﻿using Warehouse.Core.Helpers;
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
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public bool IsEmpty
        {
            get => _isEmpty;
            protected set { _isEmpty = value; OnPropertyChanged(nameof(IsEmpty)); }
        }

        public virtual BaseViewModel Parent { get;}

        #endregion

        #region Commands

        public ICommand CloseTabCommand { get; set; }
        public ICommand OpenTabCommand { get; set; }

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

        public abstract void OnPageOpen();

        #endregion
    }
}
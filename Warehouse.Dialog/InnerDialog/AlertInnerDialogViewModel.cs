﻿using Warehouse.Service.Interface;
using Warehouse.Core.Interface;

namespace Warehouse.InnerDialog;

public class AlertInnerDialogViewModel : BaseInnerDialogViewModel<bool>
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

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public AlertInnerDialogViewModel(string message, IApp app) : base(app)
    {
        Message = message;
    }

    #endregion

    #region Command Methods

    #endregion
}
using Warehouse.Core.Helpers;
using Warehouse.Service.Interface;
using Warehouse.Core.Interface;
using System.Windows.Input;
using Warehouse.Models.Enums;

namespace Warehouse.InnerDialog;

public class YesNoInnerDialogViewModel : BaseInnerDialogViewModel<EDialogResult>
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
        NoCommand = new RelayCommand(No); ;
        Result = EDialogResult.Undefind;
    }

    #endregion

    #region Command Methods

    protected override void Submit()
    {
        Result = EDialogResult.Yes;
        base.Submit();
    }

    private void No()
    {
        Result = EDialogResult.No;
        base.Submit();
    }

    #endregion
}
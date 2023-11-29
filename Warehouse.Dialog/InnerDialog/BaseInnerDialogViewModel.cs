using Warehouse.Core.Helpers;
using System.Windows.Input;
using Warehouse.ViewModel.Service;
using Warehouse.Core.Interface;
using Warehouse.Service;

namespace Warehouse.InnerDialog;

public abstract class BaseInnerDialogViewModel<T> : BaseViewModel, IBaseInnerDialogViewModel<T>, IBaseViewModel
{
    #region Private properties

    protected readonly IApp _app;
    protected bool _dialogResult = false;
    protected string _title;
    protected MessageService _message;

    #endregion

    #region Public properties

    public T? Result { get; set; }

    public bool DialogResult
    {
        get => _dialogResult;
        protected set => SetProperty(ref _dialogResult, value);
    }

    public MessageService Message
    {
        get => _message;
        protected set => SetProperty(ref _message, value);
    }

    public string Title
    {
        get => _title;
        protected set => SetProperty(ref _title, value);
    }

    #endregion

    #region Commands

    public ICommand SubmitCommand { get; protected set; }

    public ICommand ExitCommand { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public BaseInnerDialogViewModel(IApp app)
    {
        DialogResult = false;
        _app = app;
        SubmitCommand = new RelayCommand(Submit);
        ExitCommand = new RelayCommand(Exit);
        Message = new MessageService();
    }

    #endregion

    #region Command Methods

    protected virtual void Submit()
    {
        _app.GetInnerDialogService().CloseInnerDialog();
        DialogResult = true;
    }

    protected virtual void Exit()
    {
        _app.GetInnerDialogService().CloseInnerDialog();
        DialogResult = true;
    }

    #endregion
}
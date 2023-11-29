using Warehouse.Core.Interface;
using Warehouse.Models.Page;

namespace Warehouse.ViewModel.Service;

public class BasePageViewModel : Tab, IBasePageViewModel
{
    #region Private fields

    private string? _title;

    #endregion

    #region Public Properties

    public bool CanChangePage { get; protected set; } = true;

    public bool IsMain { get; set; } = false;

    public IApp Application { get; }

    public EApplicationPage Page { get; internal set; }

    public override string Title 
    { 
        get => string.IsNullOrEmpty(_title) ? Page.ToString() : _title;
        set => SetProperty(ref _title, value, nameof(Title)); 
    }

    #endregion

    #region Constructor

    public BasePageViewModel(IApp application, BaseViewModel parent) : base(parent)
    {
        Application = application;
    }

    public BasePageViewModel(IApp application) :this()
    {
        Application = application;
    }

    public BasePageViewModel() :base()
    {
        
    }

    #endregion

    #region Events

    #endregion

    #region Public methods

    public virtual void OnPageClose() { }

    public override void OnPageOpen() { }

    #endregion

}


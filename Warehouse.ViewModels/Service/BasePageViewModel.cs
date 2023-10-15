using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Service;

public class BasePageViewModel : BaseViewModel, IBasePageViewModel
{
    #region Public Properties

    public bool CanChangePage { get; protected set; } = true;

    public IApp Application { get; }

    #endregion

    #region Constructor

    public BasePageViewModel(IApp application)
    {
        Application = application;
    }

    #endregion

    #region Public methods

    public virtual void OnPageClose() { }

    public virtual void OnPageOpen() { }

    #endregion

}


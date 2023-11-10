using Warehouse.Core.Interface;
using Warehouse.Models.Page;

namespace Warehouse.ViewModel.Service;

public class BasePageViewModel : BaseViewModel, IBasePageViewModel
{
    #region Public Properties

    public bool CanChangePage { get; protected set; } = true;

    public IApp Application { get; }

    public EApplicationPage Page { get; protected set; }

    #endregion

    #region Constructor

    public BasePageViewModel(IApp application)
    {
        Application = application;
    }

    public BasePageViewModel()
    {
        
    }

    #endregion

    #region Public methods

    public virtual void OnPageClose() { }

    public virtual void OnPageOpen() { }

    #endregion

}


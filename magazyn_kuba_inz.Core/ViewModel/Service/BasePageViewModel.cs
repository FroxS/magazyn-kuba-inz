using magazyn_kuba_inz.Core.Service.Interface;

namespace magazyn_kuba_inz.Core.ViewModel.Service;

public class BasePageViewModel : BaseViewModel
{
    #region Public Properties

    public IApp Application { get; }

    #endregion

    #region Constructor

    public BasePageViewModel(IApp application)
    {
        Application = application;
    }


    #endregion
}


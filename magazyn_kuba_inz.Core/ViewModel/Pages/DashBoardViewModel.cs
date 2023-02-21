using magazyn_kuba_inz.Core.Helpers;
using magazyn_kuba_inz.Core.Service.Interface;
using magazyn_kuba_inz.Core.ViewModel.Service;
using System.Windows.Input;

namespace magazyn_kuba_inz.Core.ViewModel.Pages;

public class DashBoardViewModel : BasePageViewModel
{
    #region Public Properties


    #endregion

    #region Command

    public ICommand ExitCommand { get; private set; }

    #endregion

    #region Constructors
    public DashBoardViewModel(IApp app) : base(app)
    {
        ExitCommand = new RelayCommand((o) => {
            app.Exit();
        });

    }

    #endregion
}

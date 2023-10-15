using Warehouse.Core.Helpers;
using Warehouse.ViewModel.Service;
using System.Windows;
using System.Windows.Input;
using Warehouse.Core.Interface;

namespace Warehouse.ViewModel.Pages;

public class DashBoardPageViewModel : BasePageViewModel
{
    #region Private Fields


    #endregion

    #region Public Properties


    #endregion

    #region Command

    public ICommand ExitCommand { get; private set; }

    #endregion

    #region Constructors
    public DashBoardPageViewModel(IApp app) : base(app)
    {
        ExitCommand = new RelayCommand((o) => {
            if (app.IsAdmin)
                System.Windows.MessageBox.Show("Jest admin");
            else
                System.Windows.MessageBox.Show("Nie admin");
        });

    }

    #endregion

    #region Private Methods

    #endregion
}

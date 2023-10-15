using System.Windows;
using System.Windows.Controls;
using Warehouse.Models.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Login;

namespace Warehouse.View.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginView : Window, ILoginWindow
    {
        public LoginView(LoginViewModel vm)
        {
            DataContext = vm;
             InitializeComponent();
        }

        public IUser? GetUser()
        {
            return (DataContext as LoginViewModel)?.User;
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }

    }
}

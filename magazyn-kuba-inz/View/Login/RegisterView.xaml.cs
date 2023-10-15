using System.Windows;
using System.Windows.Controls;
using Warehouse.Core.Interface;
using Warehouse.ViewModel.Login;

namespace Warehouse.View.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class RegisterView : Window, IRegisterWindow
    {
        public RegisterView(RegisterViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {((dynamic)this.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword; }
        }

        private void PasswordBoxConfim_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).SecurePasswordConfirm = ((PasswordBox)sender).SecurePassword; }
        }
    }
}

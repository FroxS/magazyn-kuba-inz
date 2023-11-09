using System.Windows;
using Warehouse.Core.Interface;

namespace Warehouse.View.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class SplashScreen : Window, ISplashScreen
    {
        public SplashScreen()
        {
             InitializeComponent();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonPath_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

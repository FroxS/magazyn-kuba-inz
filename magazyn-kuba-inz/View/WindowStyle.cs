using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using Warehouse.Core.Interface;
using Warehouse.ViewModel;

namespace Warehouse.View
{
    public partial class WindowStyle : ResourceDictionary
    {
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2 && sender is FrameworkElement fe && fe?.DataContext is MainViewModel tbVM)
            {
                tbVM.MaximizeCommand.Execute(App.AppHost.Services.GetRequiredService<IApp>().MainWindow);
            }
            else if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                App.AppHost.Services.GetRequiredService<IApp>().MainWindow.DragMove();
            }

        }
    }
}
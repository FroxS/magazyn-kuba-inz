using System.Windows;

namespace Warehouse.Core.Interface;

public interface IWindow
{
    WindowState WindowState { get; set; }
    bool? DialogResult { get; set; }
    void Close();
    void Show();
    bool? ShowDialog();
    Window Owner { get; set; }
}

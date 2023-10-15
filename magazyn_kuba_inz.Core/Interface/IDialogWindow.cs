using System.Windows;

namespace Warehouse.Core.Interface;

public interface IDialogWindow
{
    bool? DialogResult { get; set; }
    object DataContext { get; set; }
    FrameworkElement Control { get; set; }
    bool? ShowDialog();
}
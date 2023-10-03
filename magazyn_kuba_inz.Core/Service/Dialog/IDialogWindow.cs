using System.Windows;

namespace magazyn_kuba_inz.Core.Service.Dialog
{
    public interface IDialogWindow
    {
        bool? DialogResult { get; set; }
        object DataContext { get; set; }
        FrameworkElement Control { get; set; }
        bool? ShowDialog();
    }
}
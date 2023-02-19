
using System.Windows;

namespace magazyn_kuba_inz.Core.Service.Interface
{
    public interface IWindow
    {
        WindowState WindowState { get; set; }
        bool? DialogResult { get; set; }
        void Close();
        void Show();
        bool? ShowDialog();
    }
}

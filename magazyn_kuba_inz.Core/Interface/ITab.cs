using System.Windows;
using System.Windows.Input;

namespace Warehouse.Core.Interface;

public interface ITab
{
    string Title { get; }
    ICommand CloseTabCommand { get; }
    event EventHandler CloseRequest;
    void OnPageOpen();
    void OnPageClose();
}

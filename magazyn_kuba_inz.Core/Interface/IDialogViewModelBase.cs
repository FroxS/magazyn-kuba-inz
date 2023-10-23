using System.Windows.Input;

namespace Warehouse.Core.Interface;

public interface IDialogViewModelBase<T>
{
    T DialogResult { get; set; }
    ICommand ExitCommand { get; }
    string Message { get; set; }
    ICommand OKCommand { get; }
    string Title { get; set; }

    void CloseDialogWithResult(T result);
}
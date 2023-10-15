using System.Windows.Input;

namespace Warehouse.Core.Interface;

public interface IBaseInnerDialogViewModel<T>: IBaseViewModel
{
    bool DialogResult { get; }
    ICommand ExitCommand { get; }
    T? Result { get; set; }
    ICommand SubmitCommand { get; }
}
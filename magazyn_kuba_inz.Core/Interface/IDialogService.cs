using Warehouse.Models;
using Warehouse.Models.Enums;

namespace Warehouse.Core.Interface;

public interface IDialogService
{
    T OpenDialog<T>(IDialogViewModelBase<T> type);
    void ShowAlert(string message, string title = "");
    EDialogResult AskUser(string message, string title = "");
    Product GetProduct();
    StorageUnit GetStorageUnit(string message);
    void ShowError(Exception ex);
}
using Warehouse.Models;
using Warehouse.Models.Enums;

namespace Warehouse.Core.Interface;

public interface IDialogService
{
    T OpenDialog<T>(IDialogViewModelBase<T> type);
    void ShowAlert(string message, string title = "");
   EDialogResult GetYesNoDialog(string message, string title = "");
    Product GetProduct(string message);
}
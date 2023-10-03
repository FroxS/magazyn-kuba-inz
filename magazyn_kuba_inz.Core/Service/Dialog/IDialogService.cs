using magazyn_kuba_inz.Core.ViewModel.Dialog;

namespace magazyn_kuba_inz.Core.Service.Dialog
{
    public interface IDialogService
    {
        T OpenDialog<T>(DialogViewModelBase<T> type);
        void ShowAlert(string message, string title = "");
        DialogResult GetYesNoDialog(string message, string title = "");
    }
}
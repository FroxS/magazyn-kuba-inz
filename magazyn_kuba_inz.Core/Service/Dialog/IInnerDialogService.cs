using magazyn_kuba_inz.Core.ViewModel.InnerDialog;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.WareHouse;

namespace magazyn_kuba_inz.Core.Service.Dialog
{
    public interface IInnerDialogService
    {
        BaseViewModel? InnerDialogVM { get; }
        bool IsInnerDialogOpen { get; }
        void AddProductInnerDialog(Action<Product> OnResult);
        void AddItemStateInnerDialog(Action<ItemState> OnResult);
        void AddProductGroupInnerDialog(Action<ProductGroup> OnResult);
        void AddSupplierInnerDialog(Action<Supplier> OnResult);
        void AddProductStatusInnerDialog(Action<ProductStatus> OnResult);
        void AddProductToStateInnerDialog(Product product, List<ItemState> leftStates, Action<WareHouseItem> OnResult);
        void CloseInnerDialog();
        void OpenInnerDialog<T>(BaseInnerDialogViewModel<T> vm, Action<T> OnClose);
        void OpenInnerDialog<T>(BaseInnerDialogViewModel<T> vm);
        void YesNoInnerDialog(string message, Action<DialogResult> OnResult);
        Task<StorageUnit> AddStorageUnitInnerDialog();
    }
}
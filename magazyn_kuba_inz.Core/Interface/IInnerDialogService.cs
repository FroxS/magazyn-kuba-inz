using Warehouse.Core.Models;
using Warehouse.Models;
using Warehouse.Models.Enums;

namespace Warehouse.Core.Interface;

public interface IInnerDialogService
{
    IBaseViewModel? InnerDialogVM { get; }
    bool IsInnerDialogOpen { get; }
    void AddProductInnerDialog(Action<Product> OnResult);
    void AddItemStateInnerDialog(Action<ItemState> OnResult);
    void AddProductGroupInnerDialog(Action<ProductGroup> OnResult);
    void AddSupplierInnerDialog(Action<Supplier> OnResult);
    void AddProductStatusInnerDialog(Action<ProductStatus> OnResult);
    void AddProductToStateInnerDialog(Product product, List<ItemState> leftStates, Action<WareHouseItem> OnResult);
    void CloseInnerDialog();
    void OpenInnerDialog<T>(IBaseInnerDialogViewModel<T> vm, Action<T> OnClose);
    void OpenInnerDialog<T>(IBaseInnerDialogViewModel<T> vm);
    void YesNoInnerDialog(string message, Action<EDialogResult> OnResult);

    void GetHallInnerDialog(Action<HallObject> OnResult, HallObject hall = null);
    Task<HallObject> GetHallInnerDialog(HallObject hall = null);
    Task<StorageUnit> AddStorageUnitInnerDialog();
    void GetCount(Action<double?> OnResult, double def = 1);
    Task<double?> GetCountAsync(double def = 1);
}
using Warehouse.Core.Interface;

namespace Warehouse.Core.Interface;

public interface IBasePageViewModel
{
    IApp Application { get; }
    bool CanChangePage { get; }

    void OnPageClose();
    void OnPageOpen();
}
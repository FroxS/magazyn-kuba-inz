using Warehouse.Core.Interface;
using Warehouse.Models.Page;

namespace Warehouse.Core.Interface;

public interface IBasePageViewModel
{
    IApp Application { get; }
    EApplicationPage Page { get; }
    bool CanChangePage { get; }

    void OnPageClose();
    void OnPageOpen();
}
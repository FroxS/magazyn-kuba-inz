using Warehouse.Core.Interface;
using Warehouse.Models.Page;

namespace Warehouse.Core.Interface;

public interface IBasePageViewModel : ITab
{
    IApp Application { get; }
    EApplicationPage Page { get; }
    bool CanChangePage { get; }
    bool IsMain { get; set; }
}
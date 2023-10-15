using Warehouse.Core.Delegate;
using Warehouse.Models.Page;

namespace Warehouse.Core.Interface;

public interface INavigation
{
    EApplicationPage Page { get; }
    IBasePageViewModel PageVM { get; }

    event PageChanged PageChanged;
    void SetPage(EApplicationPage page);

    void UpdateViewModel(IBasePageViewModel vm);
}
using Warehouse.Core.Delegate;
using Warehouse.Models.Page;

namespace Warehouse.Core.Interface;

public interface INavigation
{
    IBasePageViewModel Page { get; }
    bool CanSetNextPage { get; }

    bool CanSetPrevPage { get; }

    event PageChanged PageChanged;
    void SetPage(EApplicationPage page);
    void SetPage(IBasePageViewModel pageVM);
    void SetNextPage();
    void SetPrevPage();
    //void UpdateViewModel(IBasePageViewModel vm);
}
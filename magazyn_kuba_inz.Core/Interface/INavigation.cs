using System.Collections.ObjectModel;
using Warehouse.Core.Delegate;
using Warehouse.Models;
using Warehouse.Models.Page;

namespace Warehouse.Core.Interface;

public interface INavigation
{
    IBasePageViewModel Page { get; }
    ObservableCollection<IBasePageViewModel> Pages { get; }
    bool CanSetNextPage { get; }
    bool CanSetPrevPage { get; }
    IBasePageViewModel ActivePage { get; set; }
    event PageChanged PageChanged;
    void SetPage(EApplicationPage page);
    void SetNextPage();
    void SetPrevPage();
    void AddPage(IBasePageViewModel pag);
    void ChangePage(IBasePageViewModel page);
    void OpenOrder(Order order);
    IBasePageViewModel? GetOpenedOrder(Guid order);
    bool ExistOpenedOrder(Guid order);
    void CloseOrder(Order order);
    void OpenUser();
}
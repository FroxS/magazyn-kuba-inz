using System.Collections.ObjectModel;
using System.Windows.Input;
using Warehouse.Core.Delegate;
using Warehouse.Models;
using Warehouse.Models.Page;

namespace Warehouse.Core.Interface;

public interface INavigation
{
    ICommand OpenRackCommand { get; }

    IBasePageViewModel Page { get; }
    ObservableCollection<IBasePageViewModel> Pages { get; }
    bool CanSetNextPage { get; }
    bool CanSetPrevPage { get; }

    ICommand PrevCommand { get; }
    ICommand NextCommand { get; }
    IBasePageViewModel ActivePage { get; set; }
    event PageChanged PageChanged;
    void SetPage(EApplicationPage page);
    void SetPage(IBasePageViewModel pageVM);
    void SetNextPage();
    void SetPrevPage();
    void AddPage(IBasePageViewModel pag);
    void ChangePage(IBasePageViewModel page);
    void OpenOrder(Order order);
    IBasePageViewModel? GetOpenedOrder(Guid order);
    bool ExistOpenedOrder(Guid order);
    void CloseOrder(Order order);
    void OpenUser(User user = null);
    void OpenNewTabPage(ISingleCardPage page);
    void OpenRack(Guid id);
}
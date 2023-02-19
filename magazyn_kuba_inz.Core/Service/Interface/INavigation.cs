using magazyn_kuba_inz.Core.ViewModel;
using magazyn_kuba_inz.Models.Page;

namespace magazyn_kuba_inz.Core.Service.Interface
{
    public interface INavigation
    {
        ApplicationPage Page { get; }

        event PageChanged PageChanged;
        void SetPage(ApplicationPage page);
    }
}

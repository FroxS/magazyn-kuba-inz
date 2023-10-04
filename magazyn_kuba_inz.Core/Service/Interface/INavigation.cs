using magazyn_kuba_inz.Core.ViewModel;
using magazyn_kuba_inz.Core.ViewModel.Service;
using magazyn_kuba_inz.Models.Page;

namespace magazyn_kuba_inz.Core.Service.Interface
{
    public interface INavigation
    {
        EApplicationPage Page { get; }
        BasePageViewModel PageVM { get; }

        event PageChanged PageChanged;
        void SetPage(EApplicationPage page);

        void UpdateViewModel(BasePageViewModel vm);
    }
}

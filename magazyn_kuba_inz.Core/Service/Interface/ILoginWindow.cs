
using magazyn_kuba_inz.Core.ViewModel.Login;
using magazyn_kuba_inz.Models.Interfaces;

namespace magazyn_kuba_inz.Core.Service.Interface
{
    public interface ILoginWindow : IWindow
    {
        IUser? GetUser();
    }
}

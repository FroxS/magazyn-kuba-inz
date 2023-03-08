using magazyn_kuba_inz.Core.Resources;
using magazyn_kuba_inz.Models.Interfaces;

namespace magazyn_kuba_inz.Core.Service.Interface
{
    public interface IApp
    {
        IUser? User { get; }

        bool IsAdmin { get; }
        void Run();
        bool IsUserLogin();
        void LogOut();
        Task<IUser> LoginAsync(LoginResource user);
        Task Register(RegisterResource user);
        void Exit();

    }
}

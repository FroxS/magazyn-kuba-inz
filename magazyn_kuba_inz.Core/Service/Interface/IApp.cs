using magazyn_kuba_inz.Models.Interfaces;

namespace magazyn_kuba_inz.Core.Service.Interface
{
    public interface IApp
    {
        IUser? User { get; }
        void Run();
        bool IsUserLogin();
        void LogOut();
        void Login(IUser user);
        void Exit();

    }
}

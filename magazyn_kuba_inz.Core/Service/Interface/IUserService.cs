using magazyn_kuba_inz.Core.Resources;
using magazyn_kuba_inz.Models.Interfaces;

namespace magazyn_kuba_inz.Core.Service.Interface;

public interface IUserService
{
    Task<UserResource> Register(RegisterResource resource, CancellationToken cancellationToken = default(CancellationToken));
    Task<IUser> Login(LoginResource resource, CancellationToken cancellationToken = default(CancellationToken));
}

using Warehouse.Repository.Interfaces;
using Warehouse.Service.Interface;
using Warehouse.Models;
using Warehouse.Models.Interfaces;
using Warehouse.Core.Resources;
using Warehouse.Core.Helpers;
using Warehouse.Core.Exeptions;

namespace Warehouse.Service;

public class UserService : IUserService
{
    #region Private properties

    private readonly IUserRepository _repozitory;
    private readonly string _pepper;
    private readonly int _iteration = 3;

    #endregion

    #region Public properties

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public UserService(IUserRepository repozitory)
    {
        _repozitory = repozitory;
        _pepper = Environment.GetEnvironmentVariable("PasswordHashExamplePepper");
    }

    #endregion

    #region Public Method

    public async Task<UserResource> Register(RegisterResource resource, CancellationToken cancellationToken = default(CancellationToken))
    {
        var user = new User
        {
            ID = Guid.NewGuid(),
            Login = resource.Login,
            Email = resource.Email,
            PasswordSalt = PasswordHelper.GenerateSalt(),
            Active = false,
            Name = resource.Login,
            Type = Warehouse.Models.Enums.EUserType.Employee_WareHouse
        };
        user.PasswordHash = PasswordHelper.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);

        await _repozitory.AddAsync(user, cancellationToken);

        return new UserResource(user.ID, user.Login, user.Email);
    }

    public async Task<IUser> Login(LoginResource resource, CancellationToken cancellationToken = default(CancellationToken))
    {
        User? user = await _repozitory.GetByNameAsync(resource.Login, cancellationToken);

        if (user == null)
            throw new DataException("Username or password did not match.", user, "Login");

        var passwordHash = PasswordHelper.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);
        if (user.PasswordHash != passwordHash)
            throw new DataException("Username or password did not match.", user,"Login");
        
        return user;
    }

    #endregion
}
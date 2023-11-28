using Warehouse.Repository.Interfaces;
using Warehouse.Core.Interface;
using Warehouse.Models;
using Warehouse.Core.Resources;
using Warehouse.Core.Helpers;
using Warehouse.Core.Exeptions;

namespace Warehouse.Service;

internal class UserService :  BaseServiceWithRepository<IUserRepository, User>,  IUserService
{
    #region Private properties

    private readonly string _pepper;
    private readonly int _iteration = 3;

    #endregion

    #region Public properties

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public UserService(IUserRepository repozitory, IApp app): base(repozitory, app)
    {
        _pepper = Environment.GetEnvironmentVariable("PasswordHashExamplePepper");
    }

    #endregion

    #region Helpers

    private bool PassValid(string pass)
    {

        if(string.IsNullOrEmpty(pass))
            return false;

        if (pass.Length < 5 || pass.Length > 50)
            return false;

        return true;

    }

    #endregion

    #region Public Method

    public async Task<UserResource> Register(RegisterResource resource, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (!PassValid(resource.Password))
            throw new DataException("Invalid passworld", resource, nameof(resource.Password));

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

    public async Task<User> Login(LoginResource resource, CancellationToken cancellationToken = default(CancellationToken))
    {
        User? user = await _repozitory.GetByNameAsync(resource.Login, cancellationToken);

        if (user == null)
            throw new DataException("Username or password did not match.", user, "Login");

        var passwordHash = PasswordHelper.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);
        if (user.PasswordHash != passwordHash)
            throw new DataException("Username or password did not match.", user,"Login");
        
        return user;
    }

    public Task<List<User>> GetUsers(CancellationToken cancellationToken = default)
    {
        return _repozitory.GetAllAsync();
    }

    public bool ChangePassword(ChangePassworldResource resource )
    {
        if (!PassValid(resource.Password))
            throw new DataException("Invalid passworld", resource, nameof(resource.Password));

        User? user = _repozitory.GetByName(resource.Login);
        if (user == null)
            throw new DataException($"Not found User {resource.Login}") ;
        user.PasswordSalt = PasswordHelper.GenerateSalt();
        user.PasswordHash = PasswordHelper.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);
        _repozitory.Update(user);
        _repozitory.Save();
        return true;
    }

    #endregion
}
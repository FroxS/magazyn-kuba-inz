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

    public User GetUser(RegisterResource resource)
    {
        if (!PassValid(resource.Password))
            throw new DataException("Invalid passworld", resource, nameof(resource.Password));

		User user = new User
        {
            ID = Guid.NewGuid(),
            Login = resource.Login,
            Email = resource.Email,
            PasswordSalt = PasswordHelper.GenerateSalt(),
            Active = resource.IsActive,
            Name = resource.Name ?? resource.Login,
            Type = resource.type
        };
        user.PasswordHash = PasswordHelper.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);
        return user;

    }

    public async Task<User> Register(RegisterResource resource, CancellationToken cancellationToken = default(CancellationToken))
    {
        User user = GetUser(resource);

        if(GetAll().Count == 0) // First User shoud to be admin
        {
            user.Active = true;
            user.Type = Models.Enums.EUserType.Admin;
        }    

        user = await _repozitory.AddAsync(user, cancellationToken);
        if (user == null)
            throw new RegisterExeption("User not created");

        return user;
    }

    public async Task<User> Login(LoginResource resource, CancellationToken cancellationToken = default(CancellationToken))
    {
        User? user = await _repozitory.GetByLoginAsync(resource.Login, cancellationToken);

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

        User? user = _repozitory.GetByLogin(resource.Login);
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
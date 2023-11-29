using Warehouse.Repository.Interfaces;
using Warehouse.EF;
using Warehouse.Models;
using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Interface;

namespace Warehouse.Repository;

internal class UserRepository : BaseRepository<User,WarehouseDbContext>, IUserRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public UserRepository(IApp app) : base(app)
    {

    }

    #endregion

    #region public methods


    public async virtual Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Login == login);
    }


    public virtual User? GetByLogin(string login)
    {
        return _context.Users.FirstOrDefault(x => x.Login == login);
    }

    #endregion
}
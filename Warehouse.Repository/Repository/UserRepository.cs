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

    /// <summary>
    /// Method to get one Tenity from databae by Id
    /// </summary>
    /// <param name="id">Id of this Entity</param>
    /// <returns></returns>
    public async virtual Task<User> GetByNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Name == name);
    }

    #endregion
}
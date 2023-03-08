using magazyn_kuba_inz.Core.Repository.Interfaces;
using magazyn_kuba_inz.EF;
using magazyn_kuba_inz.Models.WareHouse;
using Microsoft.EntityFrameworkCore;

namespace magazyn_kuba_inz.Core.Repository;

public class UserRepository : BaseRepository<User,WarehouseDbContext>, IUserRepository
{
    #region Constructros

    /// <summary>
    /// Default constructro
    /// </summary>
    /// <param name="context">Context of database</param>
    public UserRepository(WarehouseDbContext context) : base(context)
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
        return await context.Users.FirstOrDefaultAsync(x => x.Name == name);
    }

    #endregion
}
using magazyn_kuba_inz.Models;
using Microsoft.EntityFrameworkCore;

namespace magazyn_kuba_inz.EF;

public class WarehouseDbContext: DbContext
{
    #region Public properties

    /// <summary>
    /// Table of user
    /// </summary>
    public DbSet<User> Users { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Deafult constructor
    /// </summary>
    public WarehouseDbContext() : base()
    {

    }

    /// <summary>
    /// Constructor with option
    /// </summary>
    /// <param name="options">Option od db context</param>
    public WarehouseDbContext(DbContextOptions options) : base(options)
    {

    }

    #endregion

    /// <summary>
    /// Method to on configuration db context
    /// </summary> 
    /// <param name="optionsBuilder">Option of builder</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        object value = optionsBuilder.UseSqlServer("Server=.; Database=hotelreservation; Trusted_Connection=True");
        base.OnConfiguring(optionsBuilder);
    }

}


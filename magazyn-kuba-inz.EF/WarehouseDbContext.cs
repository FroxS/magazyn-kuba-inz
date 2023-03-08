using magazyn_kuba_inz.Models.Enums;
using magazyn_kuba_inz.Models.WareHouse;
using Microsoft.EntityFrameworkCore;

namespace magazyn_kuba_inz.EF;

public class WarehouseDbContext : DbContext
{
    #region Public properties

    /// <summary>
    /// Table of user
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Table of products
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Table of suppliers
    /// </summary>
    public DbSet<Supplier> Suppliers { get; set; }

    /// <summary>
    /// Table of product status
    /// </summary>
    public DbSet<ProductStatus> ProductStatus { get; set; }

    /// <summary>
    /// Table of product groups
    /// </summary>
    public DbSet<ProductGroup> ProductGroups { get; set; }

    /// <summary>
    /// Table of warehouse items
    /// </summary>
    public DbSet<WareHouseItem> WareHouseItems { get; set; }

    /// <summary>
    /// Table of item states
    /// </summary>
    public DbSet<ItemState> ItemStates { get; set; }

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

    #region Override methods 

    /// <summary>
    /// Method to run on model createing
    /// </summary>
    /// <param name="modelBuilder">Model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(o =>
        {
            o.LoadDefaultEntity();
            o.Property(x => x.Name).IsRequired();
            o.Property(x => x.Name).HasMaxLength(100);
            o.Property(x => x.Price).IsRequired().HasDefaultValue(0);
            o.HasOne(x => x.Status)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ID_Status);
            //o.HasOne(x => x.Group)
                //.WithMany(x => x.Products)
                //.HasForeignKey(x => x.ID_Group);
            o.HasOne(x => x.Supplier)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ID_Supplier);
        });

        modelBuilder.Entity<ProductGroup>(o =>
        {
            o.LoadDefaultEntity();
            o.Property(x => x.Name).IsRequired();
            o.Property(x => x.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<ProductStatus>(o => {
            o.LoadDefaultEntity();
            o.Property(x => x.Name).IsRequired();
            o.Property(x => x.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Supplier>(o => {
            o.LoadDefaultEntity();
            o.Property(x => x.Name).IsRequired();
            o.Property(x => x.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<WareHouseItem>(o => {
            o.LoadDefaultEntity();
            o.Property(x => x.Count).IsRequired();
            o.Property(x => x.Count).HasDefaultValue(0);
            o.HasOne(x => x.State)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.ID_State);
            o.HasOne(x => x.Product)
                .WithMany(x => x.WareHouseItems)
                .HasForeignKey(x => x.ID_Product);
        });

        modelBuilder.Entity<User>(o => {
            o.LoadDefaultEntity();
            o.Property(x => x.Name);
            o.Property(x => x.Login).IsRequired().HasMaxLength(70);
            o.Property(x => x.Email).IsRequired();
            o.Property(x => x.Active).IsRequired().HasDefaultValue(false);
            o.Property(x => x.PasswordSalt).IsRequired();
            o.Property(x => x.PasswordHash).IsRequired();
            o.Property(x => x.Type).IsRequired().HasDefaultValue(UserType.Employee);
        });

        //base.OnModelCreating(modelBuilder); 
    }

    /// <summary>
    /// Method to on configuration db context
    /// </summary> 
    /// <param name="optionsBuilder">Option of builder</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        object value = optionsBuilder.UseSqlServer("Server=.; Database=magazyn; Trusted_Connection=True;TrustServerCertificate=True");
        base.OnConfiguring(optionsBuilder);
    }

    #endregion


}


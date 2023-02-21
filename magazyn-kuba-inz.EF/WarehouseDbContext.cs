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
    /// Table of orders
    /// </summary>
    public DbSet<Order> Orders { get; set; }

    /// <summary>
    /// Table of order elements
    /// </summary>
    public DbSet<OrderElement> OrderElements { get; set; }

    /// <summary>
    /// Table of products
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Table of product groups
    /// </summary>
    public DbSet<ProductGroup> ProductGroups { get; set; }

    /// <summary>
    /// Table of rack
    /// </summary>
    public DbSet<Rack> Rack { get; set; }

    /// <summary>
    /// Table of rack position
    /// </summary>
    public DbSet<RackPosition> RackPositions { get; set; }

    /// <summary>
    /// Table of storage item
    /// </summary>
    public DbSet<StorageItem> StorageItems { get; set; }

    /// <summary>
    /// Table of storageItem collection
    /// </summary>
    public DbSet<StorageItemCollection> StorageItemCollections { get; set; }

    /// <summary>
    /// Table of storage unit
    /// </summary>
    public DbSet<StorageUnit> StorageUnits { get; set; }

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
        modelBuilder.Entity<Order>(o => {
            o.HasKey(x => x.Id);
            o.Property(x => x.Id).IsRequired();
            o.Property(x => x.Name).HasMaxLength(100);
            o.Property(x => x.CreatedDate).HasDefaultValue(DateTime.Now);
            o.Property(x => x.Cost).IsRequired().HasDefaultValue(0);
            o.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.Id_User);
            o.HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.Id_Order);
        });

        modelBuilder.Entity<OrderElement>(o => {
            o.HasKey(x => x.Id);
            o.Property(x => x.Id).IsRequired();
            o.Property(x => x.Count).IsRequired().HasDefaultValue(0);
            o.HasOne(x => x.StorageItem)
                .WithMany(x => x.OrderElements)
                .HasForeignKey(x => x.Id_StorageItem);
        });

        modelBuilder.Entity<Product>(o => {
            o.HasKey(x => x.Id);
            o.Property(x => x.Id).IsRequired();
            o.Property(x => x.Name).IsRequired().HasMaxLength(250);
            o.Property(x => x.Price).IsRequired().HasPrecision(2).HasDefaultValue(0);
            o.Property(x => x.Count).IsRequired().HasDefaultValue(0);
            o.Property(x => x.Status).IsRequired().HasDefaultValue(ProductStatus.Active);
            o.HasOne(x => x.Group)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.Id_Group);
        });

        modelBuilder.Entity<ProductGroup>(o => {
            o.HasKey(x => x.Id);
            o.Property(x => x.Id).IsRequired();
            o.Property(x => x.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Rack>(o => {
            o.HasKey(x => x.Id);
            o.Property(x => x.Id).IsRequired();
            o.Property(x => x.Number);
            o.Property(x => x.Corridor);
            o.Property(x => x.Flors).IsRequired();
            o.Property(x => x.Width).IsRequired().HasDefaultValue(0);
            o.Property(x => x.Height).IsRequired().HasDefaultValue(0);
            o.Property(x => x.Depth).IsRequired().HasDefaultValue(0);
            o.Property(x => x.Direction);
            o.Property(x => x.Space_amount).IsRequired().HasDefaultValue(1);
            o.HasMany(x => x.Rack_Positions)
                .WithOne(x => x.Rack)
                .HasForeignKey(x => x.Id_Rack);
        });

        modelBuilder.Entity<RackPosition>(o => {
            o.HasKey(x => x.Id);
            o.Property(x => x.Id).IsRequired();
            o.Property(x => x.Flor).IsRequired().HasDefaultValue(0);
            o.Property(x => x.Position).IsRequired().HasDefaultValue(0);
        });

        modelBuilder.Entity<StorageItem>(o => {
            o.HasKey(x => x.Id);
            o.Property(x => x.Id).IsRequired();
            o.HasOne(x => x.StorageUnit)
                .WithMany(x => x.StorageItems)
                .HasForeignKey(x => x.Id_StorageUnit);
            o.Property(x => x.Status).IsRequired();
            o.HasOne(x => x.RackPosition)
                .WithMany(x => x.StorageItems)
                .HasForeignKey(x => x.Id_RackPosition);
            o.HasMany(x => x.StorageItems)
                .WithOne(x => x.StorageItem)
                .HasForeignKey(x => x.Id_StorageItem);
        });

        modelBuilder.Entity<StorageItemCollection>(o => {
            o.HasKey(x => x.Id);
            o.Property(x => x.Id).IsRequired();
            o.HasOne(x => x.Product)
                .WithMany(x => x.StorageItemCollections)
                .HasForeignKey(x => x.Id_Product);   
        });

        modelBuilder.Entity<StorageUnit>(o => {
            o.HasKey(x => x.Id);
            o.Property(x => x.Id).IsRequired();
            o.Property(x => x.Name).IsRequired();
            o.Property(x => x.MaxWeight).HasPrecision(3);
            o.Property(x => x.MaxHeight).HasPrecision(3);
            o.Property(x => x.MaxWidth).HasPrecision(3);
            o.Property(x => x.MaxDepth).HasPrecision(3);
            o.Property(x => x.RackUnitCapacity).HasPrecision(3).HasMaxLength(1);
        });

        modelBuilder.Entity<User>(o => {
            o.HasKey(x => x.Id);
            o.Property(x => x.Id).IsRequired();
            o.Property(x => x.Name);
            o.Property(x => x.Login).IsRequired().HasMaxLength(70);
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


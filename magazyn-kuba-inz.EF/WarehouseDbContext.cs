using Microsoft.EntityFrameworkCore;
using Warehouse.Models;
using Warehouse.Models.Enums;

namespace Warehouse.EF;

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
    /// Table of item states
    /// </summary>
    public DbSet<ItemState> ItemStates { get; set; }

    /// <summary>
    /// Table of storage item
    /// </summary>
    public DbSet<StorageItemPackage> StorageItem { get; set; }

    /// <summary>
    /// Table of rack
    /// </summary>
    public DbSet<Rack> Rack { get; set; }

    /// <summary>
    /// Table of storage unit
    /// </summary>
    public DbSet<StorageUnit> StorageUnit { get; set; }

    /// <summary>
    /// Table of items in storage item
    /// </summary>
    public DbSet<StorageItem> StorageItemCollection { get; set; }

    /// <summary>
    /// Table of orders
    /// </summary>
    public DbSet<Order> Order { get; set; }

    /// <summary>
    /// Table of orders products
    /// </summary>
    public DbSet<OrderProduct> OrderProduts { get; set; }

    /// <summary>
    /// Table of images
    /// </summary>
    public DbSet<WareHouseImage> Images { get; set; }

    /// <summary>
    /// Table of halles
    /// </summary>
    public DbSet<Hall> Halls { get; set; }

    /// <summary>
    /// Table of halles
    /// </summary>
    public DbSet<AppSettings> Settings { get; set; }

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
            o.Property(x => x.Name).IsRequired().HasMaxLength(100);
            o.Property(x => x.Price).IsRequired().HasDefaultValue(0);
            o.HasOne(x => x.Status)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ID_Status);
            o.HasOne(x => x.Supplier)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ID_Supplier);
            o.HasOne(x => x.Group)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ID_Group);
            o.HasMany(x => x.WareHouseItems)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ID_Product);
            o.HasMany(x => x.Images)
                .WithMany(x => x.Products);
        });

        modelBuilder.Entity<ProductGroup>(o =>
        {
            o.LoadDefaultEntity();
            o.Property(x => x.Name).IsRequired().HasMaxLength(255).IsUnicode();
        });

        modelBuilder.Entity<ProductStatus>(o => {
            o.LoadDefaultEntity();
            o.Property(x => x.Name).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<Supplier>(o => {
            o.LoadDefaultEntity();
            o.Property(x => x.Name).IsRequired().HasMaxLength(255);
        });

        //modelBuilder.Entity<WareHouseItem>(o => {
        //    o.LoadDefaultEntity();
        //    o.Property(x => x.Count).IsRequired();
        //    o.Property(x => x.Count).HasDefaultValue(0);
        //    o.HasOne(x => x.State)
        //        .WithMany(x => x.Items)
        //        .HasForeignKey(x => x.ID_State);
        //    o.HasOne(x => x.Product)
        //        .WithMany(x => x.WareHouseItems)
        //        .HasForeignKey(x => x.ID_Product);
        //    o.HasMany(x => x.Items)
        //        .WithOne(x => x.Item)
        //        .HasForeignKey(x => x.ID_Item);
        //});

        modelBuilder.Entity<ItemState>(o => {
            o.LoadDefaultEntity();
            o.Property(x => x.State).IsRequired();
            o.Property(x => x.Name).IsRequired();
            o.HasMany(x => x.Items)
                .WithOne(x => x.State)
                .HasForeignKey(x => x.ID_State);
        });

        modelBuilder.Entity<User>(o => {
            o.LoadDefaultEntity();
            o.Property(x => x.Name);
            o.Property(x => x.Login).IsRequired().HasMaxLength(70);
            o.Property(x => x.Email).IsRequired();
            o.Property(x => x.Active).IsRequired().HasDefaultValue(false);
            o.Property(x => x.PasswordSalt).IsRequired();
            o.Property(x => x.PasswordHash).IsRequired();
            o.Property(x => x.Type).IsRequired().HasDefaultValue(EUserType.Employee_WareHouse);
        });

        modelBuilder.Entity<StorageUnit>(o => {
            o.LoadDefaultEntity();
            o.Property(x => x.Name);
            o.Property(x => x.MaxWidth).IsRequired().HasDefaultValue(0);
            o.Property(x => x.MaxHeight).IsRequired().HasDefaultValue(0);
            o.Property(x => x.MaxWeight).IsRequired().HasDefaultValue(0);
            o.Property(x => x.MaxDepth).IsRequired().HasDefaultValue(1);
            o.Property(x => x.SizeOfRack).IsRequired().HasDefaultValue(1);
        });

        modelBuilder.Entity<Rack>(o => {
            o.LoadDefaultEntity();
            o.Property(x => x.Corridor).IsRequired().HasDefaultValue(1);
            o.Property(x => x.Flors).IsRequired().HasDefaultValue(1);
            o.Property(x => x.Width).IsRequired().HasDefaultValue(0);
            o.Property(x => x.Heigth).IsRequired().HasDefaultValue(0);
            o.Property(x => x.Deepth).IsRequired().HasDefaultValue(1);
            o.Property(x => x.Direction).IsRequired().HasDefaultValue(EDir.Left);
            o.Property(x => x.AmountSpace).IsRequired().HasDefaultValue(2);
            o.HasOne(x => x.Hall)
            .WithMany(x => x.Racks)
            .HasForeignKey(x => x.ID_Hall)
            .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Hall>(o =>
        {
            o.LoadDefaultEntity();
            o.Property(x => x.Name).IsRequired();
            o.HasMany(x => x.Racks)
            .WithOne(x => x.Hall)
            .HasForeignKey(x => x.ID_Hall)
            .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StorageItemPackage>(o => {
            o.LoadDefaultEntity();
            o.Property(x => x.Flor).HasDefaultValue(0);
            o.HasOne(x => x.Rack)
                .WithMany(x => x.StorageItems)
                .HasForeignKey(x => x.ID_Rack);
            o.HasOne(x => x.StorageUnit)
                .WithMany(x => x.Packages)
                .HasForeignKey(x => x.ID_StorageUnit);
            o.HasMany(x => x.Items)
                .WithOne(x => x.Package)
                .HasForeignKey(x => x.ID_Package);
        });

        modelBuilder.Entity<StorageItem>(o => {
            o.LoadDefaultEntity();
            o.HasOne(x => x.Package)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.ID_Package);
            o.HasOne(x => x.State)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.ID_State);
            o.HasOne(x => x.Product)
                .WithMany(x => x.WareHouseItems)
                .HasForeignKey(x => x.ID_Product);
            o.HasOne(x => x.OrderItem)
                .WithOne(x => x.StorageItem)
                .HasForeignKey<StorageItem>(x => x.ID_OrderItem)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Order>(o => {
            o.LoadDefaultEntity();
            o.Property(x => x.Name).IsRequired();
            o.Property(x => x.Margin).HasDefaultValue(0);
            o.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.ID_User);
            o.HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.ID_Order);
        });

        modelBuilder.Entity<OrderProduct>(o => {
            o.LoadDefaultEntity();
            o.HasOne(x => x.Product)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.ID_Product);
            o.HasOne(x => x.StorageItem)
                .WithOne(x => x.OrderItem)
                .HasForeignKey<StorageItem>(x => x.ID_OrderItem)
                .OnDelete(DeleteBehavior.Restrict);
            o.HasOne(x => x.Order)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.ID_Order);
        });

        modelBuilder.Entity<WareHouseImage>(o => {
            o.LoadDefaultEntity();
            o.Property(x => x.Name).IsRequired();
            o.Property(x => x.Tag);
        });

        modelBuilder.Entity<WareHouseImage>()
            .HasMany(w => w.Products)
            .WithMany(p => p.Images);

        //base.OnModelCreating(modelBuilder); 
    }

    /// <summary>
    /// Method to on configuration db context
    /// </summary> 
    /// <param name="optionsBuilder">Option of builder</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        object value = optionsBuilder.UseSqlServer("Server=.; Database=magazyn1; Trusted_Connection=True;TrustServerCertificate=True");
        base.OnConfiguring(optionsBuilder);
    }

    #endregion
}


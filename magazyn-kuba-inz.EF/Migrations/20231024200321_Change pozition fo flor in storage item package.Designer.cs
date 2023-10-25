﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Warehouse.EF;

#nullable disable

namespace Warehouse.EF.Migrations
{
    [DbContext(typeof(WarehouseDbContext))]
    [Migration("20231024200321_Change pozition fo flor in storage item package")]
    partial class Changepozitionfoflorinstorageitempackage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProductWareHouseImage", b =>
                {
                    b.Property<Guid>("ImagesID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductsID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ImagesID", "ProductsID");

                    b.HasIndex("ProductsID");

                    b.ToTable("ProductWareHouseImage");
                });

            modelBuilder.Entity("Warehouse.Models.Hall", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Hall");
                });

            modelBuilder.Entity("Warehouse.Models.ItemState", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("ItemState");
                });

            modelBuilder.Entity("Warehouse.Models.Order", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Cost")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ID_User")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RealizationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ID_User");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Warehouse.Models.OrderProduct", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ID_Order")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ID_Product")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ID_StorageItem")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("ID_Order");

                    b.HasIndex("ID_Product");

                    b.ToTable("OrderProduct");
                });

            modelBuilder.Entity("Warehouse.Models.Product", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ID_Group")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ID_Status")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ID_Supplier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double>("Price")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.HasKey("ID");

                    b.HasIndex("ID_Group");

                    b.HasIndex("ID_Status");

                    b.HasIndex("ID_Supplier");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Warehouse.Models.ProductGroup", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ID");

                    b.ToTable("ProductGroup");
                });

            modelBuilder.Entity("Warehouse.Models.ProductStatus", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ID");

                    b.ToTable("ProductStatus");
                });

            modelBuilder.Entity("Warehouse.Models.Rack", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AmountSpace")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2);

                    b.Property<int>("Corridor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("Deepth")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(1.0);

                    b.Property<int>("Direction")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Flors")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<double>("Heigth")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<Guid>("ID_Hall")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<double>("Width")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.HasKey("ID");

                    b.HasIndex("ID_Hall");

                    b.ToTable("Rack");
                });

            modelBuilder.Entity("Warehouse.Models.StorageItem", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ID_OrderItem")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ID_Product")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ID_StorageItem")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ID_OrderItem")
                        .IsUnique()
                        .HasFilter("[ID_OrderItem] IS NOT NULL");

                    b.HasIndex("ID_Product");

                    b.HasIndex("ID_StorageItem");

                    b.ToTable("StorageItem");
                });

            modelBuilder.Entity("Warehouse.Models.StorageItemPackage", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Flor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<Guid>("ID_Rack")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ID_StorageUnit")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ID_Rack");

                    b.HasIndex("ID_StorageUnit");

                    b.ToTable("StorageItemPackage");
                });

            modelBuilder.Entity("Warehouse.Models.StorageUnit", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<double>("MaxDepth")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(1.0);

                    b.Property<double>("MaxHeight")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<double>("MaxWeight")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<double>("MaxWidth")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("StorageUnit");
                });

            modelBuilder.Entity("Warehouse.Models.Supplier", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ID");

                    b.ToTable("Supplier");
                });

            modelBuilder.Entity("Warehouse.Models.User", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2);

                    b.HasKey("ID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Warehouse.Models.WareHouseImage", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Img")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("Warehouse.Models.WareHouseItem", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ID_Product")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ID_State")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ID_Product");

                    b.HasIndex("ID_State");

                    b.ToTable("WareHouseItem");
                });

            modelBuilder.Entity("ProductWareHouseImage", b =>
                {
                    b.HasOne("Warehouse.Models.WareHouseImage", null)
                        .WithMany()
                        .HasForeignKey("ImagesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Warehouse.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Warehouse.Models.Order", b =>
                {
                    b.HasOne("Warehouse.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("ID_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Warehouse.Models.OrderProduct", b =>
                {
                    b.HasOne("Warehouse.Models.Order", "Order")
                        .WithMany("Items")
                        .HasForeignKey("ID_Order")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Warehouse.Models.Product", "Product")
                        .WithMany("OrderItems")
                        .HasForeignKey("ID_Product")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Warehouse.Models.Product", b =>
                {
                    b.HasOne("Warehouse.Models.ProductGroup", "Group")
                        .WithMany("Products")
                        .HasForeignKey("ID_Group")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Warehouse.Models.ProductStatus", "Status")
                        .WithMany("Products")
                        .HasForeignKey("ID_Status")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Warehouse.Models.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("ID_Supplier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Status");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("Warehouse.Models.Rack", b =>
                {
                    b.HasOne("Warehouse.Models.Hall", "Hall")
                        .WithMany("Racks")
                        .HasForeignKey("ID_Hall")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Hall");
                });

            modelBuilder.Entity("Warehouse.Models.StorageItem", b =>
                {
                    b.HasOne("Warehouse.Models.OrderProduct", "OrderItem")
                        .WithOne("StorageItem")
                        .HasForeignKey("Warehouse.Models.StorageItem", "ID_OrderItem")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Warehouse.Models.WareHouseItem", "Product")
                        .WithMany("StorageItemCollection")
                        .HasForeignKey("ID_Product")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Warehouse.Models.StorageItemPackage", "Package")
                        .WithMany("Items")
                        .HasForeignKey("ID_StorageItem")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrderItem");

                    b.Navigation("Package");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Warehouse.Models.StorageItemPackage", b =>
                {
                    b.HasOne("Warehouse.Models.Rack", "Rack")
                        .WithMany("StorageItems")
                        .HasForeignKey("ID_Rack")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Warehouse.Models.StorageUnit", "StorageUnit")
                        .WithMany("StorageItems")
                        .HasForeignKey("ID_StorageUnit")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rack");

                    b.Navigation("StorageUnit");
                });

            modelBuilder.Entity("Warehouse.Models.WareHouseItem", b =>
                {
                    b.HasOne("Warehouse.Models.Product", "Product")
                        .WithMany("WareHouseItems")
                        .HasForeignKey("ID_Product")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Warehouse.Models.ItemState", "State")
                        .WithMany("Items")
                        .HasForeignKey("ID_State")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("State");
                });

            modelBuilder.Entity("Warehouse.Models.Hall", b =>
                {
                    b.Navigation("Racks");
                });

            modelBuilder.Entity("Warehouse.Models.ItemState", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Warehouse.Models.Order", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Warehouse.Models.OrderProduct", b =>
                {
                    b.Navigation("StorageItem");
                });

            modelBuilder.Entity("Warehouse.Models.Product", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("WareHouseItems");
                });

            modelBuilder.Entity("Warehouse.Models.ProductGroup", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Warehouse.Models.ProductStatus", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Warehouse.Models.Rack", b =>
                {
                    b.Navigation("StorageItems");
                });

            modelBuilder.Entity("Warehouse.Models.StorageItemPackage", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Warehouse.Models.StorageUnit", b =>
                {
                    b.Navigation("StorageItems");
                });

            modelBuilder.Entity("Warehouse.Models.Supplier", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Warehouse.Models.User", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Warehouse.Models.WareHouseItem", b =>
                {
                    b.Navigation("StorageItemCollection");
                });
#pragma warning restore 612, 618
        }
    }
}
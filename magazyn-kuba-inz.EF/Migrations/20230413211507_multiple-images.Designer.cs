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
    [Migration("20230413211507_multiple-images")]
    partial class multipleimages
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
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

            modelBuilder.Entity("Warehouse.WareHouse.ItemState", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ItemState");
                });

            modelBuilder.Entity("Warehouse.WareHouse.Product", b =>
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

                    b.Property<Guid>("ID_Image")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ID_Status")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ID_Supplier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

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

            modelBuilder.Entity("Warehouse.WareHouse.ProductGroup", b =>
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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ID");

                    b.ToTable("ProductGroup");
                });

            modelBuilder.Entity("Warehouse.WareHouse.ProductStatus", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ID");

                    b.ToTable("ProductStatus");
                });

            modelBuilder.Entity("Warehouse.WareHouse.Supplier", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("Lp")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ID");

                    b.ToTable("Supplier");
                });

            modelBuilder.Entity("Warehouse.WareHouse.User", b =>
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

            modelBuilder.Entity("Warehouse.WareHouse.WareHouseImage", b =>
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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("Warehouse.WareHouse.WareHouseItem", b =>
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

                    b.HasKey("ID");

                    b.HasIndex("ID_Product");

                    b.HasIndex("ID_State");

                    b.ToTable("WareHouseItem");
                });

            modelBuilder.Entity("ProductWareHouseImage", b =>
                {
                    b.HasOne("Warehouse.WareHouse.WareHouseImage", null)
                        .WithMany()
                        .HasForeignKey("ImagesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Warehouse.WareHouse.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Warehouse.WareHouse.Product", b =>
                {
                    b.HasOne("Warehouse.WareHouse.ProductGroup", "Group")
                        .WithMany("Products")
                        .HasForeignKey("ID_Group")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Warehouse.WareHouse.ProductStatus", "Status")
                        .WithMany("Products")
                        .HasForeignKey("ID_Status")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Warehouse.WareHouse.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("ID_Supplier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Status");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("Warehouse.WareHouse.WareHouseItem", b =>
                {
                    b.HasOne("Warehouse.WareHouse.Product", "Product")
                        .WithMany("WareHouseItems")
                        .HasForeignKey("ID_Product")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Warehouse.WareHouse.ItemState", "State")
                        .WithMany("Items")
                        .HasForeignKey("ID_State")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("State");
                });

            modelBuilder.Entity("Warehouse.WareHouse.ItemState", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Warehouse.WareHouse.Product", b =>
                {
                    b.Navigation("WareHouseItems");
                });

            modelBuilder.Entity("Warehouse.WareHouse.ProductGroup", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Warehouse.WareHouse.ProductStatus", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Warehouse.WareHouse.Supplier", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}

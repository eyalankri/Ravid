﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ravid.Models;

namespace Ravid.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210128100305_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Ravid.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            RoleName = "Administrator"
                        },
                        new
                        {
                            RoleId = 2,
                            RoleName = "Client"
                        });
                });

            modelBuilder.Entity("Ravid.Models.TrasportRequest", b =>
                {
                    b.Property<Guid>("TrasportRequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasColumnType("ntext");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("DeliveryFor")
                        .HasColumnType("nvarchar(400)");

                    b.Property<DateTime>("ForDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumberOfPlates")
                        .HasColumnType("int");

                    b.Property<string>("TrasportRequestStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TrasportRequestId");

                    b.HasIndex("UserId");

                    b.ToTable("TrasportRequests");
                });

            modelBuilder.Entity("Ravid.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasColumnType("ntext");

                    b.Property<string>("Company")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<DateTime>("DateRegistered")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("char(64)");

                    b.Property<string>("Phone1")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Phone2")
                        .HasColumnType("varchar(20)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("482f8b0e-1fe7-4ea6-9c10-4726a859b627"),
                            Company = "ET Internet Services",
                            DateRegistered = new DateTime(2021, 1, 28, 12, 3, 4, 981, DateTimeKind.Local).AddTicks(9353),
                            Email = "eyal.ank@gmail.com",
                            FirstName = "Eyal",
                            IsDeleted = false,
                            LastName = "Ankri",
                            Password = "744fd6f1e1f3bc2d2a023c27f4bcc1a12523767d55de7508c0b21a160ab1fdbf",
                            Phone1 = "054-6680240"
                        });
                });

            modelBuilder.Entity("Ravid.Models.UserInRole", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserInRoles");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            UserId = new Guid("482f8b0e-1fe7-4ea6-9c10-4726a859b627")
                        });
                });

            modelBuilder.Entity("Ravid.Models.TrasportRequest", b =>
                {
                    b.HasOne("Ravid.Models.User", null)
                        .WithMany("TrasportRequest")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Ravid.Models.UserInRole", b =>
                {
                    b.HasOne("Ravid.Models.Role", "Role")
                        .WithMany("UserInRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ravid.Models.User", "User")
                        .WithMany("UserInRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Ravid.Models.Role", b =>
                {
                    b.Navigation("UserInRoles");
                });

            modelBuilder.Entity("Ravid.Models.User", b =>
                {
                    b.Navigation("TrasportRequest");

                    b.Navigation("UserInRoles");
                });
#pragma warning restore 612, 618
        }
    }
}

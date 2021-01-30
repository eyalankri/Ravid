using Microsoft.EntityFrameworkCore;
using Ravid.Enums;
using Ravid.Utilities;
using System;

namespace Ravid.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<TrasportRequest> TrasportRequests { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option)
          : base(option) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Default value
            modelBuilder.Entity<User>()
                .Property(p => p.DateRegistered)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<User>()
                .Property(x => x.IsDeleted)
                .HasDefaultValue(false);


            modelBuilder.Entity<TrasportRequest>()
                .Property(p => p.DateCreated)
                .HasDefaultValueSql("getdate()");




            // Unique:
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

          
            
            //-----Primary Keys:

            // UserInRole: set both fields as primary
            modelBuilder.Entity<UserInRole>().HasKey(table => new { table.RoleId, table.UserId });


            //modelBuilder.Entity<User>()
            //    .HasMany(c => c.UserInRoles);


            //modelBuilder.Entity<Role>()
            //    .HasMany(c => c.UserInRoles);



            // ----------- Isert default data:
            // Insert Administrator & Lead Role to Role table
            modelBuilder.Entity<Role>().HasData(new Role() { RoleId = 1, RoleName = Enums.UserRoles.GetName(typeof(UserRoles), 1) }); // Admin
            modelBuilder.Entity<Role>().HasData(new Role() { RoleId = 2, RoleName = Enums.UserRoles.GetName(typeof(UserRoles), 2) }); // Client


            var salt = "";

            // Insert Admin User Eyal
            var firstAdminUserId = Guid.NewGuid();
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    UserId = firstAdminUserId,
                    FirstName = "Eyal",
                    LastName = "Ankri",
                    Email = "eyal.ank@gmail.com",
                    Password = Encryption.Sha256($"5224287ea{salt}"),
                    Phone1 = "054-6680240",
                    DateRegistered = DateTime.Now,
                    Company = "ET Internet Services"


                });


            // Insert to Eyal Ankri user
            modelBuilder.Entity<UserInRole>().HasData(
                new UserInRole()
                {
                    RoleId = (int)UserRoles.Administrator,
                    UserId = firstAdminUserId
                });






        }


       
    }
}

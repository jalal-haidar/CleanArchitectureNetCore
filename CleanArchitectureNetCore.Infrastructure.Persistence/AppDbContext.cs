using CleanArchitectureNetCore.Common.Entities;
using CleanArchitectureNetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CleanArchitectureNetCore.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        //private AuthUser _AuthUser { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Role> Roles { get; set; }
    
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Recommendation> Recommendations { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().HasMany(x => x.Recommendations).WithOne(x => x.Patient).OnDelete(DeleteBehavior.Cascade);



            #region SEED DATA
            modelBuilder.Entity<Role>().HasData(_SeedRoles);
            modelBuilder.Entity<User>().HasData(_SeedUsers);

            #endregion

            base.OnModelCreating(modelBuilder);
        }


        #region USERS
        private User[] _SeedUsers = new User[]
        {
            new User{Id=1, Email="dev@mpath.com", Password="dev", RoleId=1},
        };
        private Role[] _SeedRoles = new Role[]
        {
            new Role{Id=1, Name="Admin", CreatedOn=DateTime.MinValue, IsActive=true },
            new Role{Id=2, Name="Operator", CreatedOn=DateTime.MinValue, IsActive=true },
        };
        #endregion

        /// <summary>
        /// this method overrides saveChanges of DbContext and update <see cref="AuditableBaseEntity"/> properties
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            // detect all changes made
            ChangeTracker.DetectChanges();
            var timeStamp = DateTime.UtcNow;
            // get all newly added or modified entries detected of type 
            var entries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var entry in entries)
            {
                try
                {
                    // will throw error if the entity is not inherited from AuditableBaseEntity
                    AuditableBaseEntity entity = (AuditableBaseEntity)entry.Entity;
                    //if (entry.State == EntityState.Modified)
                    //{
                    entry.Property("LastModifiedOn").CurrentValue = timeStamp;
                    //entry.Property("LastModifiedBy").CurrentValue = _AuthUser.UserId;
                    //}

                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("CreatedOn").CurrentValue = timeStamp;
                        //entry.Property("CreatedBy").CurrentValue = _AuthUser.UserId;

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return base.SaveChanges();
        }
    }
}

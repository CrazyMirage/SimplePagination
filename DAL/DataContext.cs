using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TestHandler.DAL
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("DefaultConnection")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .Map(m =>
            {
                m.ToTable("UserRoles");
                m.MapLeftKey("UserId");
                m.MapRightKey("RoleId");
            });
        }
    }
}
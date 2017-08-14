using System.Data.Entity;
using System.Reflection;
using MyApp.DomainClasses.Roles;
using MyApp.DomainClasses.Users;
using MyApp.Framework.Data;

namespace MyApp.DataLayer.Context
{
    public class ApplicationDbContext : DbContextBase
    {
        #region Protected Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }

        #endregion

        #region Properties

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        #endregion
    }
}
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
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<UserUsedPassword> UserUsedPasswords { get; set; }


        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }

        #endregion
    }
}
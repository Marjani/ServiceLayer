using System.Data.Entity.Migrations;
using System.Data.Entity.SqlServer;
using MyApp.DataLayer.Context;
using MyApp.Framework.EntityFrameworkToolkit;

namespace MyApp.DataLayer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        private const string ApplicationdbcontextKey = "APPLICATIONDBCONTEXT_KEY";

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
            ContextKey = ApplicationdbcontextKey;

            SetSqlGenerator(SqlProviderServices.ProviderInvariantName,
                new CustomSqlServerMigrationSqlGenerator());
        }
    }
}
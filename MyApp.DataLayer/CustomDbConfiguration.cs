using MyApp.Framework.EntityFrameworkToolkit;

namespace MyApp.DataLayer
{
    public class CustomDbConfiguration : DbConfigurationBase
    {
        public CustomDbConfiguration()
        {
            // problem with user defined transaction like [db.Database.BeginTransaction()]
            //SetExecutionStrategy("System.Data.SqlClient", () => new SqlServerExecutionStrategy());

            //SetManifestTokenResolver(new CustomManifestTokenResolver());
        }
    }
}
using System.Data.Entity.Migrations;
using System.Reflection;

namespace MyApp.Framework.EntityFrameworkToolkit
{
    public abstract class DbMigrationBase : DbMigration
    {
        protected void ExecSqlResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName().Name;

            SqlResource($"{assemblyName}.Sql.{resourceName}.sql", assembly, true);
        }

        public override void Up()
        {
            ExecSqlResource("Settings");
            //ExecSqlResource("Indexes");
            //ExecSqlResource("Indexes.SqlServer");
            //ExecSqlResource("StoredProcedures");
            //ExecSqlResource("Views");
        }

        public override void Down()
        {
            //ExecSqlResource("Indexes.Inverse");
            //ExecSqlResource("Indexes.SqlServer.Inverse");
            //ExecSqlResource("StoredProcedures.Inverse");
            //ExecSqlResource("Views.Inverse");
        }
    }
}
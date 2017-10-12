using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using MyApp.DataLayer.Context;
using NUnit.Framework;

namespace MyApp.IntegrationTests
{
    [SetUpFixture]
    public class TestSetup
    {
        [OneTimeSetUp]
        public void SetUpDatabase()
        {
            DestroyDatabase();
            CreateDatabase();
        }

        [OneTimeTearDown]
        public void TearDownDatabase()
        {
            DestroyDatabase();
        }

        private static void CreateDatabase()
        {
            ExecuteSqlCommand(Master, $"CREATE DATABASE [MyAppTest]  ON (NAME = 'MyAppTest', FILENAME = '{FileName}') COLLATE Persian_100_CI_AS;");
            ExecuteSqlCommand(Master, "ALTER DATABASE [MyAppTest] SET ALLOW_SNAPSHOT_ISOLATION ON;" +
                                      " ALTER DATABASE[MyAppTest] SET READ_COMMITTED_SNAPSHOT ON; ");
            var migration =
                new MigrateDatabaseToLatestVersion<ApplicationDbContext, DataLayer.Migrations.Configuration>();
            migration.InitializeDatabase(new ApplicationDbContext());

            //ExecuteSqlCommand(MyAppTest, SqlResources.V1_0_0);
        }

        private static void DestroyDatabase()
        {
            var fileNames = ExecuteSqlQuery(Master, @"
                SELECT [physical_name] FROM [sys].[master_files]
                WHERE [database_id] = DB_ID('MyAppTest')",
                row => (string)row["physical_name"]);

            if (!fileNames.Any()) return;

            ExecuteSqlCommand(Master, @"
                    ALTER DATABASE [MyAppTest] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    EXEC sp_detach_db 'MyAppTest' , 'true'");

            fileNames.ForEach(File.Delete);
        }

        private static void ExecuteSqlCommand(DbConnectionStringBuilder sqlConnectionStringBuilder, string commandText)
        {
            using (var connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }

        private static List<T> ExecuteSqlQuery<T>(DbConnectionStringBuilder sqlConnectionStringBuilder, string queryText, Func<SqlDataReader, T> read)
        {
            var result = new List<T>();
            using (var connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = queryText;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(read(reader));
                        }
                    }
                }
            }
            return result;
        }

        private static SqlConnectionStringBuilder Master =>
            new SqlConnectionStringBuilder
            {
                DataSource = @"(LocalDB)\MSSQLLocalDB",
                InitialCatalog = "master",
                IntegratedSecurity = true
            };
        private static SqlConnectionStringBuilder MyAppTest =>
            new SqlConnectionStringBuilder
            {
                DataSource = @"(LocalDB)\MSSQLLocalDB",
                InitialCatalog = "MyAppTest",
                IntegratedSecurity = true
            };
        private static string FileName => Path.Combine(
            Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location),
            "MyAppTest.mdf");
    }
}

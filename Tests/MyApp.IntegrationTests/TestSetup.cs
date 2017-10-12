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
            ExecuteSqlCommand(Master, string.Format(SqlResource.DatabaseScript, FileName));

            //Use T-Sql Scripts For Create Database
            //ExecuteSqlCommand(MyAppTest, SqlResources.V1_0_0);

            var migration =
                new MigrateDatabaseToLatestVersion<ApplicationDbContext, DataLayer.Migrations.Configuration>();
            migration.InitializeDatabase(new ApplicationDbContext());

        }

        private static void DestroyDatabase()
        {
            var fileNames = ExecuteSqlQuery(Master, SqlResource.SelecDatabaseFileNames,
                row => (string)row["physical_name"]);

            if (!fileNames.Any()) return;

            ExecuteSqlCommand(Master, SqlResource.DetachDatabase);

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

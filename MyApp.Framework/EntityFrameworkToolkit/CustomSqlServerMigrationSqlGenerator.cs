﻿using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using System.Linq;
using MyApp.Framework.EntityFrameworkToolkit.FullTextSearch;
using MyApp.Framework.EntityFrameworkToolkit.Indexing;
using MyApp.Framework.GuardToolkit;
using MyApp.Framework.LinqToolkit;

namespace MyApp.Framework.EntityFrameworkToolkit
{
    public class CustomSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(MigrationOperation migrationOperation)
        {
            Guard.ArgumentNotNull(migrationOperation, nameof(migrationOperation));

            var fullTextIndexOperation = migrationOperation as CreateFullTextIndexOperation;
            if (fullTextIndexOperation != null)
            {
                GenerateFullTextIndex(fullTextIndexOperation);
                return;
            }

            var includedColumnsIndexOperation = migrationOperation as CreateIncludedColumnsIndexOperation;
            if (includedColumnsIndexOperation != null)
            {
                GenerateIncludedColumnsIndex(includedColumnsIndexOperation);
                return;
            }

            base.Generate(migrationOperation);
        }

        private void GenerateIncludedColumnsIndex(CreateIncludedColumnsIndexOperation includedColumnIndexOperation)
        {
            using (var writer = Writer())
            {
                writer.Write("CREATE ");
                if (includedColumnIndexOperation.IsUnique)
                    writer.Write("UNIQUE ");
                if (includedColumnIndexOperation.IsClustered)
                    writer.Write("CLUSTERED ");
                writer.Write("INDEX ");
                writer.Write(Quote(includedColumnIndexOperation.Name));
                writer.Write(" ON ");
                writer.Write(Name(includedColumnIndexOperation.Table));
                writer.Write("(");
                writer.Write(includedColumnIndexOperation.Columns.Join(Quote, ", "));
                writer.Write(")");
                writer.Write("INCLUDE");
                writer.Write("(");
                writer.Write(includedColumnIndexOperation.IncludedColumns.Join(Quote, ", "));
                writer.Write(")");
                Statement(writer);
            }
        }

        private void GenerateFullTextIndex(CreateFullTextIndexOperation fullTextIndexOperation)
        {
            using (var writer = Writer())
            {
                writer.WriteLine("IF(NOT EXISTS(SELECT * FROM SYS.fulltext_catalogs WHERE is_default = 1))");
                writer.WriteLine("BEGIN");
                writer.WriteLine("    CREATE FULLTEXT CATALOG DefaultFullTextCatalog AS DEFAULT");
                writer.WriteLine("END");

                writer.WriteLine();

                writer.WriteLine(
                    $"CREATE FULLTEXT INDEX ON {Name(fullTextIndexOperation.Table)} ({string.Join(", ", fullTextIndexOperation.Columns.Select(Quote))})");
                writer.WriteLine($"KEY INDEX {Quote(fullTextIndexOperation.Name)}");
                writer.WriteLine("WITH CHANGE_TRACKING AUTO");

                Statement(writer.InnerWriter.ToString(), true);
            }
        }
    }
}
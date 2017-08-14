using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using MyApp.Framework.EntityFrameworkToolkit.FullTextSearch;
using MyApp.Framework.EntityFrameworkToolkit.Indexing;

namespace MyApp.Framework.EntityFrameworkToolkit.Extensions
{
    public static class DbMigrationExtensions
    {
        public static void CreateIncludedColumnsIndex(
            this DbMigration migration,
            string table,
            string index,
            IList<string> columns,
            IList<string> includedColumns,
            bool isUnique = false,
            bool isClustered = false
        )
        {
            var op = new CreateIncludedColumnsIndexOperation
            {
                Table = table,
                Name = index,
                Columns = columns,
                IncludedColumns = includedColumns,
                IsUnique = isUnique,
                IsClustered = isClustered
            };

            ((IDbMigration) migration).AddOperation(op);
        }

        public static void CreateFullTextIndex(
            this DbMigration migration,
            string table,
            string index,
            IEnumerable<string> columns)
        {
            var op = new CreateFullTextIndexOperation
            {
                Table = table,
                Name = index,
                Columns = columns
            };

            ((IDbMigration) migration).AddOperation(op);
        }
    }
}
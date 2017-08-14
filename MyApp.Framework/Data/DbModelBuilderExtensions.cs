using System.Data.Entity;
using EntityFramework.DynamicFilters;
using MyApp.Framework.Domain.Entities;

namespace MyApp.Framework.Data
{
    internal static class DbModelBuilderExtensions
    {
        public static void ConfigureFrameworkFilters(this DbModelBuilder modelBuilder)
        {
            modelBuilder.Filter(nameof(IHasRowLevelSecurity),
                (IHasRowLevelSecurity rls, long userId) => rls.UserId == userId, 0);
            modelBuilder.Filter(nameof(ISoftDeletable), (ISoftDeletable d) => d.IsDeleted, false);
            modelBuilder.Filter(nameof(ISystemDefaultEntry), (ISystemDefaultEntry e) => e.IsSystemEntry, true);
        }
    }
}
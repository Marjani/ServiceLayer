using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MyApp.Framework.EntityFrameworkToolkit.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, int skip, int take)
        {
            return queryable.Skip(() => skip).Take(() => take);
        }

        public static IQueryable<T> IncludeProperties<T>(this IQueryable<T> queryable,
            params Expression<Func<T, object>>[] includeProperties)
        {
            if (queryable == null)
                throw new ArgumentNullException(nameof(queryable));

            return includeProperties.Aggregate(queryable,
                (current, includeProperty) => current.Include(includeProperty));
        }
        public static IQueryable IncludeIf(this IQueryable source, bool condition, string path)
        {
            return condition
                ? source.Include(path)
                : source;
        }

        public static IQueryable<T> IncludeIf<T>(this IQueryable<T> source, bool condition, string path)
        {
            return condition
                ? source.Include(path)
                : source;
        }

        public static IQueryable<T> IncludeIf<T, TProperty>(this IQueryable<T> source, bool condition, Expression<Func<T, TProperty>> path)
        {
            return condition
                ? source.Include(path)
                : source;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MyApp.Framework.Application.Models;
using MyApp.Framework.EntityFrameworkToolkit.Extensions;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace MyApp.Framework.Application
{
    public static class QueryableExtensions
    {
        #region ToListResponse

        public static DynamicListResponse ToListResponse<T>(this IQueryable<T> queryable, int take, int skip,
            IEnumerable<Sort> sort, Filter filter, IEnumerable<Aggregator> aggregates)
        {
            queryable = queryable.Filter(filter);

            var total = queryable.LongCount();

            var aggregate = queryable.Aggregate(aggregates);

            queryable = queryable.Sort(sort);

            if (take > 0)
                queryable = queryable.Page(skip, take);

            return new DynamicListResponse
            {
                Data = queryable.ToList(),
                Total = total,
                Aggregates = aggregate
            };
        }

        public static DynamicListResponse ToListResponse<T>(this IQueryable<T> queryable, int take, int skip,
            IEnumerable<Sort> sort, Filter filter)
        {
            return queryable.ToListResponse(take, skip, sort, filter, null);
        }

        public static DynamicListResponse ToListResponse<T>(this IQueryable<T> queryable, DynamicListRequest request)
        {
            return queryable.ToListResponse(request.Take, request.Skip, request.Sorts, request.Filter, null);
        }

        #endregion

        #region ToListResponseAsync

        public static async Task<DynamicListResponse> ToListResponseAsync<T>(this IQueryable<T> queryable, int take,
            int skip,
            IEnumerable<Sort> sort, Filter filter, IEnumerable<Aggregator> aggregates)
        {
            queryable = queryable.Filter(filter);

            var total = await queryable.LongCountAsync();

            var aggregate = queryable.Aggregate(aggregates);

            queryable = queryable.Sort(sort);

            if (take > 0)
                queryable = queryable.Page(skip, take);

            return new DynamicListResponse
            {
                Data = await queryable.ToListAsync(),
                Total = total,
                Aggregates = aggregate
            };
        }

        public static Task<DynamicListResponse> ToListResponseAsync<T>(this IQueryable<T> queryable, int take, int skip,
            IEnumerable<Sort> sort, Filter filter)
        {
            return queryable.ToListResponseAsync(take, skip, sort, filter, null);
        }

        public static Task<DynamicListResponse> ToListResponseAsync<T>(this IQueryable<T> queryable,
            DynamicListRequest request)
        {
            return queryable.ToListResponseAsync(request.Take, request.Skip, request.Sorts, request.Filter, null);
        }

        #endregion

        #region Filter,Aggregate,Sort

        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, Filter filter)
        {
            if (filter?.Logic == null) return queryable;

            var filters = filter.All();

            // Get all filter values as array (needed by the Where method of Dynamic Linq)
            var values = filters.Select(f => f.Value).ToArray();

            // Create a predicate expression e.g. Field1 = @0 And Field2 > @1
            var predicate = filter.ToExpression(filters);

            // Use the Where method of Dynamic Linq to filter the data
            queryable = queryable.Where(predicate, values);

            return queryable;
        }

        public static object Aggregate<T>(this IQueryable<T> queryable, IEnumerable<Aggregator> aggregates)
        {
            if (aggregates == null) return null;

            var aggregators = aggregates as Aggregator[] ?? aggregates.ToArray();
            if (!aggregators.Any()) return null;

            var objProps = new Dictionary<DynamicProperty, object>();
            var groups = aggregators.GroupBy(g => g.Field);
            Type type;
            foreach (var group in groups)
            {
                var fieldProps = new Dictionary<DynamicProperty, object>();
                foreach (var aggregate in group)
                {
                    var prop = typeof(T).GetProperty(aggregate.Field);
                    if (prop == null)
                        throw new InvalidOperationException($"{aggregate.Field} Property does not exists!");

                    var param = Expression.Parameter(typeof(T), "s");
                    var selector = aggregate.Aggregate == "count" &&
                                   Nullable.GetUnderlyingType(prop.PropertyType) != null
                        ? Expression.Lambda(
                            Expression.NotEqual(Expression.MakeMemberAccess(param, prop),
                                Expression.Constant(null, prop.PropertyType)), param)
                        : Expression.Lambda(Expression.MakeMemberAccess(param, prop), param);
                    var mi = aggregate.MethodInfo(typeof(T));
                    if (mi == null)
                        continue;

                    var val = queryable.Provider.Execute(Expression.Call(null, mi,
                        aggregate.Aggregate == "count" && Nullable.GetUnderlyingType(prop.PropertyType) == null
                            ? new[] {queryable.Expression}
                            : new[] {queryable.Expression, Expression.Quote(selector)}));

                    fieldProps.Add(new DynamicProperty(aggregate.Aggregate, typeof(object)), val);
                }
                type = DynamicExpression.CreateClass(fieldProps.Keys);
                var fieldObj = Activator.CreateInstance(type);
                foreach (var p in fieldProps.Keys)
                    type.GetProperty(p.Name).SetValue(fieldObj, fieldProps[p], null);
                objProps.Add(new DynamicProperty(group.Key, fieldObj.GetType()), fieldObj);
            }

            type = DynamicExpression.CreateClass(objProps.Keys);

            var obj = Activator.CreateInstance(type);

            foreach (var p in objProps.Keys)
                type.GetProperty(p.Name).SetValue(obj, objProps[p], null);

            return obj;
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, IEnumerable<Sort> sort)
        {
            if (sort == null) return queryable;

            var sorts = sort as Sort[] ?? sort.ToArray();
            if (!sorts.Any()) return queryable;

            var ordering = string.Join(",", sorts.Select(s => s.ToExpression()));

            return queryable.OrderBy(ordering);
        }

        #endregion
    }
}
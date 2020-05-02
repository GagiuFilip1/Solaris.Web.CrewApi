using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;

namespace Solaris.Web.CrewApi.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<Tuple<int, List<TSource>>> WithPaginationAsync<TSource>(this IQueryable<TSource> source, Pagination paging)
        {
            return new Tuple<int, List<TSource>>(
                await source.CountAsync(),
                await source.Skip(paging.Offset).Take(paging.Take)
                    .AsQueryable()
                    .ToListAsync());
        }

        public static IQueryable<TSource> WithOrdering<TSource>(this IQueryable<TSource> source, Ordering ordering, Ordering defaultOrdering)
        {
            if (ordering == null || string.IsNullOrEmpty(ordering.OrderBy))
                ordering = defaultOrdering;

            return source.OrderBy(ordering.OrderBy, ordering.OrderDirection);
        }

        private static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName, OrderDirection direction)
        {
            var entity = typeof(TSource);
            var propertyInfo = entity.GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
            var arg = Expression.Parameter(entity, "x");
            var property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, arg);

            var actionName = direction.Equals(OrderDirection.Asc)
                ? "OrderBy"
                : "OrderByDescending";

            var method = typeof(Queryable).GetMethods()
                .Where(m => m.Name == actionName && m.IsGenericMethodDefinition)
                .Single(m =>
                {
                    var parameters = m.GetParameters().ToList();
                    return parameters.Count == 2;
                });

            if (propertyInfo == null)
                return query;

            var genericMethod = method.MakeGenericMethod(entity, propertyInfo.PropertyType);

            var newQuery = (IOrderedQueryable<TSource>) genericMethod.Invoke(genericMethod, new object[] {query, selector});
            return newQuery;
        }
    }
}
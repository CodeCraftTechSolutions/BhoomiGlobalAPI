using BhoomiGlobalAPI.HelperClass;
using System.Linq.Expressions;

namespace BhoomiGlobal.Service.Extension
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, QueryObject queryObject, Dictionary<string, Expression<Func<T, object>>> columnsMap)
        {
            if (queryObject.IsSortAsc)
            {
                return query.OrderBy(columnsMap[queryObject.SortBy]).Skip((queryObject.Page-1) * queryObject.PageSize).Take(queryObject.PageSize);
            }
            else
            {
                return query.OrderByDescending(columnsMap[queryObject.SortBy]).Skip((queryObject.Page - 1) * queryObject.PageSize).Take(queryObject.PageSize);
            }
        }

        public static IQueryable<T> ApplyOrderingBool<T>(this IQueryable<T> query, QueryObject queryObject, Dictionary<string, Expression<Func<T, bool>>> columnsMap)
        {
            if (queryObject.IsSortAsc)
            {
                return query.OrderBy(columnsMap[queryObject.SortBy]).Skip((queryObject.Page - 1) * queryObject.PageSize).Take(queryObject.PageSize);
            }
            else
            {
                return query.OrderByDescending(columnsMap[queryObject.SortBy]).Skip((queryObject.Page - 1) * queryObject.PageSize).Take(queryObject.PageSize);
            }
        }

        public static IQueryable<T> ApplyOrderingOnly<T>(this IQueryable<T> query, QueryObject queryObject, Dictionary<string, Expression<Func<T, object>>> columnsMap)
        {
            if (queryObject.IsSortAsc)
            {
                return query.OrderBy(columnsMap[queryObject.SortBy]);
            }
            else
            {
                return query.OrderByDescending(columnsMap[queryObject.SortBy]);
            }
        }

        public static IQueryable<T> ApplyOrderingDateTime<T>(this IQueryable<T> query, QueryObject queryObject, Dictionary<string, Expression<Func<T, DateTime>>> columnsMap)
        {
            if (queryObject.IsSortAsc)
            {
                return query.OrderBy(columnsMap[queryObject.SortBy]).Skip((queryObject.Page - 1) * queryObject.PageSize).Take(queryObject.PageSize);
            }
            else
            {
                return query.OrderByDescending(columnsMap[queryObject.SortBy]).Skip((queryObject.Page - 1) * queryObject.PageSize).Take(queryObject.PageSize);
            }
        }

        public static IQueryable<T> ApplyOrderingDecimal<T>(this IQueryable<T> query, QueryObject queryObject, Dictionary<string, Expression<Func<T, decimal?>>> columnsMap)
        {
            if (queryObject.IsSortAsc)
            {
                return query.OrderBy(columnsMap[queryObject.SortBy]).Skip((queryObject.Page - 1) * queryObject.PageSize).Take(queryObject.PageSize);
            }
            else
            {
                return query.OrderByDescending(columnsMap[queryObject.SortBy]).Skip((queryObject.Page - 1) * queryObject.PageSize).Take(queryObject.PageSize);
            }
        }


    }
}

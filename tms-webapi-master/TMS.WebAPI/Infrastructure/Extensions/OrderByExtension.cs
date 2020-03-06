using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TMS.Web.Infrastructure.Extensions
{
    public static class OrderByExtension
    {
        /// <summary>Sorts By field in ascending order or descending according to a key for IQueryable.</summary>
        /// <param name="q">Querry</param>
        /// <param name="sortField">Colunm of field</param>
        /// <param name="isAsc">Ascending</param>
        /// <typeparam name="T"></typeparam>
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string
            sortField, bool isAsc)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, sortField);
            var exp = Expression.Lambda(prop, param);
            string method = isAsc ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types,
                q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }

        /// <summary>Sorts By field in ascending order or descending according to a key for IEnumerable.</summary>
        /// <param name="q">Querry</param>
        /// <param name="sortField">Colunm of field</param>
        /// <param name="isAsc">Ascending</param>
        /// <typeparam name="T"></typeparam>
        public static IEnumerable<TEntity> OrderByField<TEntity>(this IEnumerable<TEntity> source,
                                                    string orderByProperty, bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command,
                                                   new[] { type, property.PropertyType },
                                                   source.AsQueryable().Expression,
                                                   Expression.Quote(orderByExpression));
            return source.AsQueryable().Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
using System;
using System.Linq;
using System.Linq.Expressions;

namespace UoC_API.Extentions
{
    public static class OrderingExtentions
    {
        public static IOrderedQueryable<T> ComprehensiveStringSort<T>(this IQueryable<T> source, string sortByName, bool sortByAsc, string thenByName, bool thenByAsc)
        {
            if(sortByAsc && thenByAsc)
            {
                return source.OrderBy(ToLambda<T>(sortByName)).ThenBy(ToLambda<T>(thenByName));
            }
            else if(!sortByAsc && thenByAsc)
            {
                return source.OrderBy(ToLambda<T>(sortByName)).ThenByDescending(ToLambda<T>(thenByName));
            }
            else if (sortByAsc && !thenByAsc)
            {
                return source.OrderByDescending(ToLambda<T>(sortByName)).ThenBy(ToLambda<T>(thenByName));
            }
            else
            {
                return source.OrderByDescending(ToLambda<T>(sortByName)).ThenByDescending(ToLambda<T>(thenByName));
            }
        }


        public static IOrderedQueryable<T> OrderByString<T>(this IQueryable<T> source, string propertryName)
        {
            return source.OrderBy(ToLambda<T>(propertryName));
        }

        public static IOrderedQueryable<T> OrderByStringDescending<T>(this IQueryable<T> source, string propertryName)
        {
            return source.OrderByDescending(ToLambda<T>(propertryName));
        }

        public static IOrderedQueryable<T> ThenByString<T>(this IOrderedQueryable<T> source, string thenByPropertyName)
        {
            return source.ThenBy(ToLambda<T>(thenByPropertyName));
        }

        public static IOrderedQueryable<T> ThenByStringDescending<T>(this IOrderedQueryable<T> source, string thenByPropertyName)
        {
            return source.ThenByDescending(ToLambda<T>(thenByPropertyName));
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

    }
}

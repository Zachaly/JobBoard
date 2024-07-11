using JobBoard.Domain;
using JobBoard.Model;
using JobBoard.Model.Attributes;
using JobBoard.Model.Enum;
using System.Linq.Expressions;
using System.Reflection;

namespace JobBoard.Database
{
    internal static class IQueryableExtensions
    {
        public static IQueryable<TEntity> AddPagination<TEntity>(this IQueryable<TEntity> query, PagedRequest request)
            where TEntity : IEntity
        {
            if(request.SkipPagination.GetValueOrDefault())
            {
                return query;
            }

            var page = request.PageIndex ?? 0;
            var pageSize = request.PageSize ?? 10;

            return query.Skip(page * pageSize).Take(pageSize);
        }

        public static IQueryable<TEntity> FilterWithRequest<TEntity, TRequest>(this IQueryable<TEntity> query, PagedRequest request)
            where TEntity : IEntity
            where TRequest : PagedRequest
        {
            var entityProps = typeof(TEntity).GetProperties();
            var requestProps = typeof(TRequest)
                .GetProperties()
                .Where(p => entityProps.Any(x => x.Name == p.Name) || p.GetCustomAttribute<CustomFilterAttribute>() is not null)
                .Where(p => p.GetValue(request) is not null);

            var entityParam = Expression.Parameter(typeof(TEntity), "entity");

            foreach(var prop in requestProps)
            {
                var attribute = prop.GetCustomAttribute<CustomFilterAttribute>();

                var propName = prop.Name;

                if(attribute is not null && !string.IsNullOrEmpty(attribute.Property))
                {
                    propName = attribute.Property;  
                }

                var entityPropExpression = Expression.Property(entityParam, propName);

                if(attribute is not null && !string.IsNullOrEmpty(attribute.SubProperty))
                {
                    entityPropExpression = Expression.Property(entityPropExpression, attribute.SubProperty);
                }

                var requestPropExpression = Expression.Constant(prop.GetValue(request));

                Expression comparisonExpression;

                if(attribute is not null)
                {
                    MethodInfo? methodInfo = null;

                    if(attribute.ComparisonType == ComparisonType.StartsWith)
                    {
                        methodInfo = prop.PropertyType.GetMethod("StartsWith", [typeof(string)]);
                    }
                    else if(attribute.ComparisonType == ComparisonType.Contains || attribute.ComparisonType == ComparisonType.DoesNotContain)
                    {
                        methodInfo = prop.PropertyType.GetMethod("Contains");
                    }

                    comparisonExpression = attribute.ComparisonType switch
                    {
                        ComparisonType.Equal => Expression.Equal(entityPropExpression, requestPropExpression),
                        ComparisonType.NotEqual => Expression.NotEqual(entityPropExpression, requestPropExpression),
                        ComparisonType.Lesser => Expression.LessThan(entityPropExpression, requestPropExpression),
                        ComparisonType.LesserOrEqual => Expression.LessThanOrEqual(entityPropExpression, requestPropExpression),
                        ComparisonType.Greater => Expression.GreaterThan(entityPropExpression, requestPropExpression),
                        ComparisonType.GreaterOrEqual => Expression.GreaterThanOrEqual(entityPropExpression, requestPropExpression),
                        ComparisonType.StartsWith => Expression.Call(entityPropExpression, methodInfo!, requestPropExpression),
                        ComparisonType.Contains => Expression.Call(requestPropExpression, methodInfo!, entityPropExpression),
                        ComparisonType.DoesNotContain => Expression.Equal(Expression.Call(requestPropExpression, methodInfo!,
                            entityPropExpression), Expression.Constant(false)),
                        _ => throw new NotSupportedException(),
                    };
                }
                else
                {
                    comparisonExpression = Expression.Equal(entityPropExpression, requestPropExpression);
                }

                var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(comparisonExpression, entityParam);

                query = query.Where(lambdaExpression);
            }

            return query;
        }
    }
}

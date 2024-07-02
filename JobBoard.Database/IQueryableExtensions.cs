using JobBoard.Domain;
using JobBoard.Model;
using System.Linq.Expressions;

namespace JobBoard.Database
{
    internal static class IQueryableExtensions
    {
        public static IQueryable<TEntity> AddPagination<TEntity>(this IQueryable<TEntity> query, PagedRequest request)
            where TEntity : IEntity
        {
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
                .Where(p => entityProps.Any(x => x.Name == p.Name))
                .Where(p => p.GetValue(request) is not null);

            var entityParam = Expression.Parameter(typeof(TEntity), "entity");

            foreach(var prop in requestProps)
            {
                var entityPropExpression = Expression.Property(entityParam, prop.Name);

                var requestPropExpression = Expression.Constant(prop.GetValue(request));

                var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(entityPropExpression, requestPropExpression),
                    entityParam);

                query = query.Where(lambdaExpression);
            }

            return query;
        }
    }
}

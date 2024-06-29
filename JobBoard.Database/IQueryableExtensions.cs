using JobBoard.Domain;
using JobBoard.Model;

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
    }
}

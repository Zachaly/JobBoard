using JobBoard.Domain;
using JobBoard.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoard.Database.Repository.Abstraction
{
    public interface IRepositoryBase<TEntity, TModel, TGetRequest>
        where TEntity : IEntity
        where TModel : class
        where TGetRequest : PagedRequest
    {
        Task<IEnumerable<TModel>> GetAsync(TGetRequest request);
        Task<TModel?> GetByIdAsync(long id);
        Task<TEntity?> GetEntityByIdAsync(long id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteByIdAsync(long id);
    }

    public abstract class RepositoryBase<TEntity, TModel, TGetRequest> : IRepositoryBase<TEntity, TModel, TGetRequest>
        where TEntity : class, IEntity
        where TModel : class
        where TGetRequest : PagedRequest
    {
        protected readonly ApplicationDbContext _dbContext;
        protected Expression<Func<TEntity, TModel>> ModelExpression;

        protected RepositoryBase(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(long id)
        {
            var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);

            if(entity is null)
            {
                return;
            }

            _dbContext.Set<TEntity>().Remove(entity);

            await _dbContext.SaveChangesAsync();
        }

        public Task<IEnumerable<TModel>> GetAsync(TGetRequest request)
        {
            var query = _dbContext.Set<TEntity>()
                .FilterWithRequest<TEntity, TGetRequest>(request)
                .AddPagination(request)
                .Select(ModelExpression)
                .AsEnumerable();

            return Task.FromResult(query);
        }

        public Task<TModel?> GetByIdAsync(long id)
            => _dbContext.Set<TEntity>().Where(e => e.Id == id).Select(ModelExpression).FirstOrDefaultAsync();

        public Task<TEntity?> GetEntityByIdAsync(long id)
            => _dbContext.Set<TEntity>().Where(e => e.Id == id).FirstOrDefaultAsync();

        public Task UpdateAsync(TEntity entity)
        {
            _dbContext.Update(entity);

            return _dbContext.SaveChangesAsync();
        }
    }
}

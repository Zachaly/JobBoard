using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Expressions;
using JobBoard.Model.Business;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Database.Repository
{
    public class BusinessRepository : RepositoryBase<Business, BusinessModel, GetBusinessRequest>, IBusinessRepository
    {
        public BusinessRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = BusinessExpressions.Model;
        }

        public Task<Business?> GetByNameAsync(string name)
            => _dbContext.Set<Business>().FirstOrDefaultAsync(x => x.Name == name);
    }
}

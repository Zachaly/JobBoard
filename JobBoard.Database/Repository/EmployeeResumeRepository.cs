using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Expressions;
using JobBoard.Model.EmployeeResume;

namespace JobBoard.Database.Repository
{
    public class EmployeeResumeRepository : RepositoryBase<EmployeeResume, EmployeeResumeModel, GetEmployeeResumeRequest>,
        IEmployeeResumeRepository
    {
        public EmployeeResumeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = EmployeeResumeExpressions.Model;
        }
    }
}

using JobBoard.Domain.Entity;
using JobBoard.Model.EmployeeResume;

namespace JobBoard.Database.Repository.Abstraction
{
    public interface IEmployeeResumeRepository : IRepositoryBase<EmployeeResume, EmployeeResumeModel, GetEmployeeResumeRequest>
    {
    }
}

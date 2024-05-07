using JobBoard.Domain.Entity;
using JobBoard.Model.EmployeeAccount;

namespace JobBoard.Application.Factory.Abstraction
{
    public interface IEmployeeAccountFactory
    {
        EmployeeAccount Create(AddEmployeeAccountRequest request, string passwordHash);
    }
}

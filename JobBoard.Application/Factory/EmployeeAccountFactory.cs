using JobBoard.Application.Factory.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.EmployeeAccount;

namespace JobBoard.Application.Factory
{
    public class EmployeeAccountFactory : IEmployeeAccountFactory
    {
        public EmployeeAccount Create(AddEmployeeAccountRequest request, string passwordHash)
            => new EmployeeAccount
            {
                AboutMe = request.AboutMe,
                City = request.City,
                Country = request.Country,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = passwordHash,
                PhoneNumber = request.PhoneNumber,
            };
    }
}

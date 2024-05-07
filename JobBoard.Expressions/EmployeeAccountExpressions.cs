using JobBoard.Domain.Entity;
using JobBoard.Model.EmployeeAccount;
using System.Linq.Expressions;

namespace JobBoard.Expressions
{
    public static class EmployeeAccountExpressions
    {
        public static Expression<Func<EmployeeAccount, EmployeeAccountModel>> Model { get; } = account => new EmployeeAccountModel
        {
            AboutMe = account.AboutMe,
            City = account.City,
            Country = account.Country,
            Email = account.Email,
            FirstName = account.FirstName,
            Id = account.Id,
            LastName = account.LastName,
            PhoneNumber = account.PhoneNumber,
        };
    }
}

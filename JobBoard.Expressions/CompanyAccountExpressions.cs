using JobBoard.Domain.Entity;
using JobBoard.Model.CompanyAccount;
using System.Linq.Expressions;

namespace JobBoard.Expressions
{
    public static class CompanyAccountExpressions
    {
        public static Expression<Func<CompanyAccount, CompanyModel>> Model { get; } = account => new CompanyModel
        {
            Address = account.Address,
            City = account.City,
            ContactEmail = account.ContactEmail,
            Country = account.Country,
            Id = account.Id,
            Name = account.Name,
            PostalCode = account.PostalCode,
        };
    }
}

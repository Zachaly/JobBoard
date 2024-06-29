using JobBoard.Application.Factory.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.CompanyAccount;

namespace JobBoard.Application.Factory
{
    public class CompanyAccountFactory : ICompanyAccountFactory
    {
        public CompanyAccount Create(AddCompanyAccountRequest request, string passwordHash)
            => new CompanyAccount
            {
                Address = request.Address,
                City = request.City,
                ContactEmail = request.ContactEmail,
                Country = request.Country,
                Email = request.Email,
                PasswordHash = passwordHash,
                Name = request.Name,
                PostalCode = request.PostalCode,
            };
    }
}

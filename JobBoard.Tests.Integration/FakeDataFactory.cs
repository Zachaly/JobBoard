using Bogus;
using JobBoard.Domain.Entity;

namespace JobBoard.Tests.Integration
{
    internal static class FakeDataFactory
    {
        public static List<CompanyAccount> CreateCompanyAccounts(int count)
            => new Faker<CompanyAccount>()
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Address, f => f.Address.StreetAddress())
                .RuleFor(x => x.PostalCode, f => f.Address.ZipCode())
                .RuleFor(x => x.ContactEmail, f => f.Internet.Email())
                .RuleFor(x => x.Country, f => f.Address.CountryCode())
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.Name, f => f.Name.JobTitle())
                .RuleFor(x => x.PasswordHash, _ => "hash")
                .Generate(count);

        public static List<EmployeeAccount> CreateEmployeeAccounts(int count)
            => new Faker<EmployeeAccount>()
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.PasswordHash, f => f.Random.String())
                .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                .RuleFor(x => x.LastName, f => f.Name.LastName())
                .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber().Replace(" ", ""))
                .RuleFor(x => x.PasswordHash, _ => "hash")
                .Generate(count);

        public static List<AdminAccount> CreateAdminAccounts(int count) 
            => new Faker<AdminAccount>()
                .RuleFor(x => x.Login, f => f.Name.FirstName())
                .RuleFor(x => x.PasswordHash, f => f.Random.String())
                .Generate(count);

        public static List<JobOffer> CreateJobOffers(long companyId, int count)
            => new Faker<JobOffer>()
                .RuleFor(x => x.Title, f => f.Random.Words(2))
                .RuleFor(x => x.CreationDate, _ => DateTimeOffset.UtcNow)
                .RuleFor(x => x.ExpirationDate, _ => DateTimeOffset.UtcNow.AddDays(1))
                .RuleFor(x => x.Description, f => f.Random.Words(5))
                .RuleFor(x => x.Location, f => f.Address.City())
                .RuleFor(x => x.CompanyId, _ => companyId)
                .Generate(count);

        public static List<JobOfferRequirement> CreateJobOfferRequirements(long offerId, int count)
            => new Faker<JobOfferRequirement>()
                .RuleFor(x => x.OfferId, _ => offerId)
                .RuleFor(x => x.Content, f => f.Random.Words(5))
                .Generate(count);
    }
}

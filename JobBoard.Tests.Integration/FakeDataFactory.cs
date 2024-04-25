using Bogus;
using JobBoard.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .RuleFor(x => x.Password, _ => "hash")
                .Generate(count);
    }
}

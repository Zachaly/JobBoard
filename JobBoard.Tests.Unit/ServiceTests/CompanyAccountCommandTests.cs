using FluentValidation;
using FluentValidation.Results;
using JobBoard.Application.Command;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.CompanyAccount;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace JobBoard.Tests.Unit.ServiceTests
{
    public class CompanyAccountCommandTests
    {

        [Fact]
        public async Task AddCompanyAccountCommand_ValidRequest_AddsAccount()
        {
            var request = new AddCompanyAccountCommand();

            var repository = Substitute.For<ICompanyAccountRepository>();

            var hashService = Substitute.For<IHashService>();

            var factory = Substitute.For<ICompanyAccountFactory>();

            var validator = Substitute.For<IValidator<AddCompanyAccountRequest>>();

            var createdAccount = new CompanyAccount();

            const string GeneratedHash = "hash";

            repository.GetByEmailAsync(request.Email).ReturnsNull();

            repository.AddAsync(createdAccount).Returns(Task.CompletedTask);

            hashService.HashPassword(request.Password).Returns(GeneratedHash);

            factory.Create(request, GeneratedHash).Returns(createdAccount);

            validator.Validate(request).Returns(new ValidationResult());

            var handler = new AddCompanyAccountHandler(repository, factory, validator, hashService);

            var res = await handler.Handle(request, default);

            await repository.Received(1).AddAsync(createdAccount);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task AddCompanyAccountCommand_InvalidRequest_Failure()
        {
            var request = new AddCompanyAccountCommand();

            var repository = Substitute.For<ICompanyAccountRepository>();

            var hashService = Substitute.For<IHashService>();

            var factory = Substitute.For<ICompanyAccountFactory>();

            var validator = Substitute.For<IValidator<AddCompanyAccountRequest>>();

            validator.Validate(request).Returns(new ValidationResult(new List<ValidationFailure>()
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new AddCompanyAccountHandler(repository, factory, validator, hashService);

            var res = await handler.Handle(request, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors!);
        }

        [Fact]
        public async Task AddCompanyAccountCommand_ValidRequest_EmailTaken_Failure()
        {
            var request = new AddCompanyAccountCommand();

            var repository = Substitute.For<ICompanyAccountRepository>();

            var hashService = Substitute.For<IHashService>();

            var factory = Substitute.For<ICompanyAccountFactory>();

            var validator = Substitute.For<IValidator<AddCompanyAccountRequest>>();

            repository.GetByEmailAsync(request.Email).Returns(new CompanyAccount());

            validator.Validate(request).Returns(new ValidationResult());

            var handler = new AddCompanyAccountHandler(repository, factory, validator, hashService);

            var res = await handler.Handle(request, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task GetCompanyAccountCommand_ReturnsAccounts()
        {
            var accounts = new List<CompanyModel>()
            {
                new CompanyModel(), new CompanyModel(), new CompanyModel()
            };

            var request = new GetCompanyAccountCommand();

            var repository = Substitute.For<ICompanyAccountRepository>();

            repository.GetAsync(request).Returns(accounts);

            var handler = new GetCompanyAccountHandler(repository);

            var res = await handler.Handle(request, default);

            Assert.Equal(accounts, res);
        }

        [Fact]
        public async Task GetCompanyAccountByIdCommand_ReturnsSpecifiedAccount()
        {
            const long Id = 1;

            var model = new CompanyModel();

            var repository = Substitute.For<ICompanyAccountRepository>();

            repository.GetByIdAsync(Id).Returns(model);

            var handler = new GetCompanyAccountByIdHandler(repository);

            var res = await handler.Handle(new GetCompanyAccountByIdCommand(Id), default);

            Assert.Equal(model, res);
        }

        [Fact]
        public async Task CompanyLoginCommand_ReturnsResponseWithTokenAndId()
        {
            var request = new CompanyLoginCommand();

            var account = new CompanyAccount
            {
                Id = 1,
            };

            const string Token = "token";

            var repository = Substitute.For<ICompanyAccountRepository>();
            repository.GetByEmailAsync(request.Login).Returns(account);

            var hashService = Substitute.For<IHashService>();
            hashService.VerifyPassword(request.Password, account.Password).Returns(true);

            var tokenService = Substitute.For<ITokenService>();
            tokenService.GenerateTokenAsync(account.Id, "Company").Returns(Token);

            var handler = new CompanyLoginHandler(repository, hashService, tokenService);

            var res = await handler.Handle(request, default);

            Assert.True(res.IsSuccess);
            Assert.Equal(account.Id, res.UserId);
            Assert.Equal(Token, res.AuthToken);
        }

        [Fact]
        public async Task CompanyLoginCommand_UserNotFound_Failure()
        {
            var request = new CompanyLoginCommand();

            var repository = Substitute.For<ICompanyAccountRepository>();
            repository.GetByEmailAsync(request.Login).ReturnsNull();

            var hashService = Substitute.For<IHashService>();

            var tokenService = Substitute.For<ITokenService>();

            var handler = new CompanyLoginHandler(repository, hashService, tokenService);

            var res = await handler.Handle(request, default);

            Assert.False(res.IsSuccess);
            Assert.Equal(0, res.UserId);
            Assert.Empty(res.AuthToken);
        }

        [Fact]
        public async Task CompanyLoginCommand_PasswordDoNotMatch_Failure()
        {
            var request = new CompanyLoginCommand();

            var account = new CompanyAccount
            {
                Id = 1,
            };

            var repository = Substitute.For<ICompanyAccountRepository>();
            repository.GetByEmailAsync(request.Login).Returns(account);

            var hashService = Substitute.For<IHashService>();
            hashService.VerifyPassword(request.Password, account.Password).Returns(false);

            var tokenService = Substitute.For<ITokenService>();

            var handler = new CompanyLoginHandler(repository, hashService, tokenService);

            var res = await handler.Handle(request, default);

            Assert.False(res.IsSuccess);
            Assert.Equal(0, res.UserId);
            Assert.Empty(res.AuthToken);
        }
    }
}

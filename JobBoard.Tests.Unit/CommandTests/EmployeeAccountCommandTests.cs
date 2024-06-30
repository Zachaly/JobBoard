using FluentValidation;
using FluentValidation.Results;
using JobBoard.Application.Command;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.EmployeeAccount;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace JobBoard.Tests.Unit.CommandTests
{
    public class EmployeeAccountCommandTests
    {
        [Fact]
        public async Task GetEmployeeAccountCommand_ReturnsAccounts()
        {
            var list = new List<EmployeeAccountModel>
            {
                new EmployeeAccountModel(),
                new EmployeeAccountModel()
            };

            var command = new GetEmployeeAccountCommand();

            var repository = Substitute.For<IEmployeeAccountRepository>();

            repository.GetAsync(command).Returns(list);

            var handler = new GetEmployeeAccountHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.Equal(list, res);
        }

        [Fact]
        public async Task GetEmployeeAccountByIdCommand_ReturnsAccount()
        {
            var account = new EmployeeAccountModel();

            var command = new GetEmployeeAccountByIdCommand(1);

            var repository = Substitute.For<IEmployeeAccountRepository>();

            repository.GetByIdAsync(command.Id).Returns(account);

            var handler = new GetEmployeeAccountByIdHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.Equal(account, res);
        }

        [Fact]
        public async Task AddEmployeeAccountCommand_AddsAccount()
        {
            var command = new AddEmployeeAccountCommand
            {
                Password = "pass",
                Email = "email"
            };

            var account = new EmployeeAccount();

            const string Hash = "pass";

            var repository = Substitute.For<IEmployeeAccountRepository>();

            repository.GetByEmailAsync(command.Email).ReturnsNull();
            repository.AddAsync(account).Returns(Task.CompletedTask);

            var factory = Substitute.For<IEmployeeAccountFactory>();

            factory.Create(command, Hash).Returns(account);

            var hashService = Substitute.For<IHashService>();

            hashService.HashPassword(command.Password).Returns(Hash);

            var validator = Substitute.For<IValidator<AddEmployeeAccountRequest>>();

            validator.Validate(command).Returns(new ValidationResult());

            var handler = new AddEmployeeAccountHandler(factory, repository, validator, hashService);

            var res = await handler.Handle(command, default);

            await repository.Received(1).AddAsync(account);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task AddEmployeeAccountCommand_InvalidRequest_Failure()
        {
            var command = new AddEmployeeAccountCommand
            {
                Password = "pass",
                Email = "email"
            };

            var repository = Substitute.For<IEmployeeAccountRepository>();

            var factory = Substitute.For<IEmployeeAccountFactory>();

            var hashService = Substitute.For<IHashService>();

            var validator = Substitute.For<IValidator<AddEmployeeAccountRequest>>();

            validator.Validate(command).Returns(new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("prop", "msg") }));

            var handler = new AddEmployeeAccountHandler(factory, repository, validator, hashService);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task AddEmployeeAccountCommand_EmailTaken_Failure()
        {
            var command = new AddEmployeeAccountCommand
            {
                Password = "pass",
                Email = "email"
            };

            var repository = Substitute.For<IEmployeeAccountRepository>();

            repository.GetByEmailAsync(command.Email).Returns(new EmployeeAccount());

            var factory = Substitute.For<IEmployeeAccountFactory>();

            var hashService = Substitute.For<IHashService>();

            var validator = Substitute.For<IValidator<AddEmployeeAccountRequest>>();

            validator.Validate(command).Returns(new ValidationResult());

            var handler = new AddEmployeeAccountHandler(factory, repository, validator, hashService);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotNull(res.Error);
        }

        [Fact]
        public async Task EmployeeLoginCommand_ReturnsResponseWithTokenAndId()
        {
            var request = new EmployeeLoginCommand();

            var account = new EmployeeAccount
            {
                Id = 1,
            };

            const string Token = "token";
            const string RefreshToken = "reftokne";

            var repository = Substitute.For<IEmployeeAccountRepository>();
            repository.GetByEmailAsync(request.Login).Returns(account);

            var hashService = Substitute.For<IHashService>();
            hashService.VerifyPassword(request.Password, account.PasswordHash).Returns(true);

            var tokenService = Substitute.For<IAccessTokenService>();
            tokenService.GenerateTokenAsync(account.Id, "Employee").Returns(Token);

            var refreshTokenService = Substitute.For<IRefreshTokenService>();
            refreshTokenService.GenerateEmployeeAccountTokenAsync(account.Id).Returns(RefreshToken);

            var handler = new EmployeeLoginHandler(repository, tokenService, hashService, refreshTokenService);

            var res = await handler.Handle(request, default);

            Assert.True(res.IsSuccess);
            Assert.Equal(account.Id, res.UserId);
            Assert.Equal(Token, res.AuthToken);
            Assert.Equal(RefreshToken, res.RefreshToken);
        }

        [Fact]
        public async Task EmployeeLoginCommand_UserNotFound_Failure()
        {
            var request = new EmployeeLoginCommand();

            var repository = Substitute.For<IEmployeeAccountRepository>();
            repository.GetByEmailAsync(request.Login).ReturnsNull();

            var hashService = Substitute.For<IHashService>();

            var tokenService = Substitute.For<IAccessTokenService>();

            var refreshTokenService = Substitute.For<IRefreshTokenService>();

            var handler = new EmployeeLoginHandler(repository, tokenService, hashService, refreshTokenService);

            var res = await handler.Handle(request, default);

            Assert.False(res.IsSuccess);
            Assert.Equal(0, res.UserId);
            Assert.Empty(res.AuthToken);
            Assert.Empty(res.RefreshToken);
        }

        [Fact]
        public async Task EmployeeLoginCommand_PasswordDoNotMatch_Failure()
        {
            var request = new EmployeeLoginCommand();

            var account = new EmployeeAccount
            {
                Id = 1,
            };

            var repository = Substitute.For<IEmployeeAccountRepository>();
            repository.GetByEmailAsync(request.Login).Returns(account);

            var hashService = Substitute.For<IHashService>();
            hashService.VerifyPassword(request.Password, account.PasswordHash).Returns(false);

            var tokenService = Substitute.For<IAccessTokenService>();

            var refreshTokenService = Substitute.For<IRefreshTokenService>();

            var handler = new EmployeeLoginHandler(repository, tokenService, hashService, refreshTokenService);

            var res = await handler.Handle(request, default);

            Assert.False(res.IsSuccess);
            Assert.Equal(0, res.UserId);
            Assert.Empty(res.AuthToken);
            Assert.Empty(res.RefreshToken);
        }

        [Fact]
        public async Task UpdateEmployeeAccountCommand_UpdatesEntity()
        {
            var command = new UpdateEmployeeAccountCommand
            {
                Id = 1,
                AboutMe = "about",
                City = "city",
                Country = "country",
                FirstName = "fname",
                LastName = "lname",
                PhoneNumber = "phone"
            };

            var account = new EmployeeAccount();

            var repository = Substitute.For<IEmployeeAccountRepository>();
            repository.GetEntityByIdAsync(command.Id).Returns(account);
            repository.UpdateAsync(account).Returns(Task.CompletedTask);

            var validator = Substitute.For<IValidator<UpdateEmployeeAccountRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new UpdateEmployeeAccountHandler(repository, validator);

            var res = await handler.Handle(command, default);

            await repository.Received(1).UpdateAsync(account);

            Assert.True(res.IsSuccess);
            Assert.Equal(command.AboutMe, account.AboutMe);
            Assert.Equal(command.LastName, account.LastName);
            Assert.Equal(command.FirstName, account.FirstName);
            Assert.Equal(command.Country, account.Country);
            Assert.Equal(command.PhoneNumber, account.PhoneNumber);
            Assert.Equal(command.City, account.City);
        }

        [Fact]
        public async Task UpdateEmployeeAccountCommand_InvalidRequest_Failure()
        {
            var command = new UpdateEmployeeAccountCommand();

            var repository = Substitute.For<IEmployeeAccountRepository>();

            var validator = Substitute.For<IValidator<UpdateEmployeeAccountRequest>>();
            validator.Validate(command).Returns(new ValidationResult(new List<ValidationFailure>()
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new UpdateEmployeeAccountHandler(repository, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task UpdateEmployeeAccountCommand_EntityNotFound_Failure()
        {
            var command = new UpdateEmployeeAccountCommand();

            var repository = Substitute.For<IEmployeeAccountRepository>();
            repository.GetEntityByIdAsync(command.Id).ReturnsNull();

            var validator = Substitute.For<IValidator<UpdateEmployeeAccountRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new UpdateEmployeeAccountHandler(repository, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }
    }
}

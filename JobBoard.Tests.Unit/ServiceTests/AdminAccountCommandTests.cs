using FluentValidation;
using FluentValidation.Results;
using JobBoard.Application.Command;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.AdminAccount;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace JobBoard.Tests.Unit.ServiceTests
{
    public class AdminAccountCommandTests
    {
        [Fact]
        public async Task GetAdminAccountCommand_ReturnsAccounts()
        {
            var accounts = new List<AdminAccountModel>
            {
                new AdminAccountModel { Id = 1 },
                new AdminAccountModel { Id = 2 },
            };

            var command = new GetAdminAccountCommand();

            var repository = Substitute.For<IAdminAccountRepository>();

            repository.GetAsync(command).Returns(accounts);

            var handler = new GetAdminAccountHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.Equivalent(accounts, res);
        }

        [Fact]
        public async Task AddAdminAccountCommand_ValidRequest_AccountAdded()
        {
            var command = new AddAdminAccountCommand();

            const string Hash = "hash";

            var createdAccount = new AdminAccount();

            var repository = Substitute.For<IAdminAccountRepository>();

            repository.GetByLoginAsync(command.Login).ReturnsNull();
            repository.AddAsync(createdAccount).Returns(Task.CompletedTask);

            var factory = Substitute.For<IAdminAccountFactory>();
            factory.Create(command, Hash).Returns(createdAccount);

            var hashService = Substitute.For<IHashService>();
            hashService.HashPassword(command.Login).Returns(Hash);

            var validator = Substitute.For<IValidator<AddAdminAccountRequest>>();

            validator.Validate(command).Returns(new ValidationResult());

            var handler = new AddAdminAccountHandler(repository, factory, hashService, validator);

            var res = await handler.Handle(command, default);

            await repository.Received(1).AddAsync(createdAccount);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task AddAdminAccountCommand_InvalidRequest_Failure()
        {
            var command = new AddAdminAccountCommand();

            var repository = Substitute.For<IAdminAccountRepository>();

            var factory = Substitute.For<IAdminAccountFactory>();

            var hashService = Substitute.For<IHashService>();

            var validator = Substitute.For<IValidator<AddAdminAccountRequest>>();

            validator.Validate(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new AddAdminAccountHandler(repository, factory, hashService, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task AddAdminAccountCommand_LoginTaken_Failure()
        {
            var command = new AddAdminAccountCommand();

            var repository = Substitute.For<IAdminAccountRepository>();

            repository.GetByLoginAsync(command.Login).Returns(new AdminAccount());

            var factory = Substitute.For<IAdminAccountFactory>();

            var hashService = Substitute.For<IHashService>();

            var validator = Substitute.For<IValidator<AddAdminAccountRequest>>();

            validator.Validate(command).Returns(new ValidationResult());

            var handler = new AddAdminAccountHandler(repository, factory, hashService, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task GetAdminAccountByIdCommand_ReturnsAdminAccount()
        {
            var command = new GetAdminAccountByIdCommand(1);

            var account = new AdminAccountModel();

            var repository = Substitute.For<IAdminAccountRepository>();

            repository.GetByIdAsync(command.Id).Returns(account);

            var handler = new GetAdminAccountByIdHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.Equal(account, res);
        }
    }
}

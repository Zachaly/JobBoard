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

namespace JobBoard.Tests.Unit.ServiceTests
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
    }
}

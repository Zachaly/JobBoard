using FluentValidation;
using FluentValidation.Results;
using JobBoard.Application.Command;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.Business;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;

namespace JobBoard.Tests.Unit.CommandTests
{
    public class BusinessCommandTests
    {
        [Fact]
        public async Task GetBusinessCommand_ReturnsBusinesses()
        {
            var command = new GetBusinessCommand();

            var businesses = new List<BusinessModel>
            {
                new BusinessModel { Id = 1, Name = "name"}
            };

            var repository = Substitute.For<IBusinessRepository>();

            repository.GetAsync(command).Returns(businesses);

            var handler = new GetBusinessHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.Equal(businesses, res);
        }

        [Fact]
        public async Task AddBusinessCommand_AddsBusiness()
        {
            var command = new AddBusinessCommand
            {
                Name = "name"
            };

            var createdEntity = new Business();

            var repository = Substitute.For<IBusinessRepository>();
            repository.GetByNameAsync(command.Name).ReturnsNull();
            repository.AddAsync(createdEntity).Returns(Task.CompletedTask);

            var factory = Substitute.For<IBusinessFactory>();
            factory.Create(command).Returns(createdEntity);

            var validator = Substitute.For<IValidator<AddBusinessRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new AddBusinessHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            await repository.Received(1).AddAsync(createdEntity);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task AddBusinessCommand_InvalidCommand_Failure()
        {
            var command = new AddBusinessCommand
            {
                Name = "name"
            };

            var repository = Substitute.For<IBusinessRepository>();

            var factory = Substitute.For<IBusinessFactory>();

            var validator = Substitute.For<IValidator<AddBusinessRequest>>();
            validator.Validate(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new AddBusinessHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task AddBusinessCommand_NameTaken_Failure()
        {
            var command = new AddBusinessCommand
            {
                Name = "name"
            };

            var repository = Substitute.For<IBusinessRepository>();
            repository.GetByNameAsync(command.Name).Returns(new Business());

            var factory = Substitute.For<IBusinessFactory>();

            var validator = Substitute.For<IValidator<AddBusinessRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new AddBusinessHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task UpdateBusinessCommand_UpdatesBusiness()
        {
            var command = new UpdateBusinessCommand
            {
                Id = 1,
                Name = "name",
            };

            var business = new Business();

            var repository = Substitute.For<IBusinessRepository>();
            repository.GetEntityByIdAsync(command.Id).Returns(business);
            repository.GetByNameAsync(command.Name).ReturnsNull();
            repository.UpdateAsync(business).Returns(Task.CompletedTask);

            var factory = Substitute.For<IBusinessFactory>();

            var validator = Substitute.For<IValidator<UpdateBusinessRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new UpdateBusinessHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            await repository.Received(1).UpdateAsync(business);
            factory.Received(1).Update(business, command);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task UpdateBusinessCommand_InvalidCommand_Failure()
        {
            var command = new UpdateBusinessCommand
            {
                Id = 1,
                Name = "name",
            };

            var repository = Substitute.For<IBusinessRepository>();

            var factory = Substitute.For<IBusinessFactory>();

            var validator = Substitute.For<IValidator<UpdateBusinessRequest>>();
            validator.Validate(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new UpdateBusinessHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]    
        public async Task UpdateBusinessCommand_EntityNotFound_Failure()
        {
            var command = new UpdateBusinessCommand
            {
                Id = 1,
                Name = "name",
            };

            var repository = Substitute.For<IBusinessRepository>();
            repository.GetEntityByIdAsync(command.Id).ReturnsNull();

            var factory = Substitute.For<IBusinessFactory>();

            var validator = Substitute.For<IValidator<UpdateBusinessRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new UpdateBusinessHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task UpdateBusinessCommand_NameTaken_Failure()
        {
            var command = new UpdateBusinessCommand
            {
                Id = 1,
                Name = "name",
            };

            var repository = Substitute.For<IBusinessRepository>();
            repository.GetByNameAsync(command.Name).Returns(new Business());

            var factory = Substitute.For<IBusinessFactory>();

            var validator = Substitute.For<IValidator<UpdateBusinessRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new UpdateBusinessHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task DeleteBusinessByIdCommand_DeletesBusiness()
        {
            var command = new DeleteBusinessByIdCommand(1);

            var repository = Substitute.For<IBusinessRepository>();
            repository.DeleteByIdAsync(command.Id).Returns(Task.CompletedTask);

            var handler = new DeleteBusinessByIdHandler(repository);

            var res = await handler.Handle(command, default);

            await repository.Received(1).DeleteByIdAsync(command.Id);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task DeleteBusinessByIdCommand_ExceptionThrown_Failure()
        {
            var command = new DeleteBusinessByIdCommand(1);

            var repository = Substitute.For<IBusinessRepository>();
            repository.DeleteByIdAsync(command.Id).Throws(new Exception("msg"));

            var handler = new DeleteBusinessByIdHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }
    }
}

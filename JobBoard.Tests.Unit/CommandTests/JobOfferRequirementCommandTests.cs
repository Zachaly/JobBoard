using FluentValidation;
using FluentValidation.Results;
using JobBoard.Application.Command;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferRequirement;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Tests.Unit.CommandTests
{
    public class JobOfferRequirementCommandTests
    {
        [Fact]
        public async Task GetJobOfferRequirementCommand_ReturnsRequirements()
        {
            var command = new GetJobOfferRequirementCommand();

            var requirements = new List<JobOfferRequirementModel>
            {
                new JobOfferRequirementModel(),
                new JobOfferRequirementModel()
            };

            var repository = Substitute.For<IJobOfferRequirementRepository>();
            repository.GetAsync(command).Returns(requirements);

            var handler = new GetJobOfferRequirementHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.Equivalent(requirements, res);
        }

        [Fact]
        public async Task AddJobOfferRequirementCommand_AddsRequirement()
        {
            var command = new AddJobOfferRequirementCommand();

            var requirement = new JobOfferRequirement();

            var repository = Substitute.For<IJobOfferRequirementRepository>();
            repository.AddAsync(requirement).Returns(Task.CompletedTask);

            var factory = Substitute.For<IJobOfferRequirementFactory>();
            factory.Create(command).Returns(requirement);

            var validator = Substitute.For<IValidator<AddJobOfferRequirementRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new AddJobOfferRequirementHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            await repository.Received(1).AddAsync(requirement);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task AddJobOfferRequirementCommand_ValidationFailed_Failure()
        {
            var command = new AddJobOfferRequirementCommand();

            var repository = Substitute.For<IJobOfferRequirementRepository>();

            var factory = Substitute.For<IJobOfferRequirementFactory>();

            var validator = Substitute.For<IValidator<AddJobOfferRequirementRequest>>();
            validator.Validate(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new AddJobOfferRequirementHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task UpdateJobOfferRequirementCommand_UpdatesRequirement()
        {
            var command = new UpdateJobOfferRequirementCommand();

            var requirement = new JobOfferRequirement();

            var repository = Substitute.For<IJobOfferRequirementRepository>();
            repository.GetEntityByIdAsync(command.Id).Returns(requirement);
            repository.UpdateAsync(requirement).Returns(Task.CompletedTask);

            var factory = Substitute.For<IJobOfferRequirementFactory>();
            
            var validator = Substitute.For<IValidator<UpdateJobOfferRequirementRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new UpdateJobOfferRequirementHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            await repository.Received(1).UpdateAsync(requirement);
            factory.Received(1).Update(requirement, command);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task UpdateJobOfferRequirementCommand_ValidationFailed_Failure()
        {
            var command = new UpdateJobOfferRequirementCommand();

            var repository = Substitute.For<IJobOfferRequirementRepository>();

            var factory = Substitute.For<IJobOfferRequirementFactory>();

            var validator = Substitute.For<IValidator<UpdateJobOfferRequirementRequest>>();
            validator.Validate(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new UpdateJobOfferRequirementHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task UpdateJobOfferRequirementCommand_EntityNotFound_Failure()
        {
            var command = new UpdateJobOfferRequirementCommand();

            var repository = Substitute.For<IJobOfferRequirementRepository>();
            repository.GetEntityByIdAsync(command.Id).ReturnsNull();

            var factory = Substitute.For<IJobOfferRequirementFactory>();

            var validator = Substitute.For<IValidator<UpdateJobOfferRequirementRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new UpdateJobOfferRequirementHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task DeleteJobOfferRequirementByIdCommand_DeletesRequirement()
        {
            var command = new DeleteJobOfferRequirementByIdCommand(1);

            var repository = Substitute.For<IJobOfferRequirementRepository>();
            repository.DeleteByIdAsync(command.Id).Returns(Task.CompletedTask);

            var handler = new DeleteJobOfferRequirementByIdHandler(repository);

            var res = await handler.Handle(command, default);

            await repository.Received(1).DeleteByIdAsync(command.Id);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task DeleteJobOfferRequirementByIdCommand_ExceptionThrown_Failure()
        {
            var command = new DeleteJobOfferRequirementByIdCommand(1);

            var repository = Substitute.For<IJobOfferRequirementRepository>();
            repository.DeleteByIdAsync(command.Id).Throws(new Exception("message"));

            var handler = new DeleteJobOfferRequirementByIdHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }
    }
}

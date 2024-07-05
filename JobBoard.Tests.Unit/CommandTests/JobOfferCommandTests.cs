using FluentValidation;
using FluentValidation.Results;
using JobBoard.Application.Command;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.JobOffer;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;

namespace JobBoard.Tests.Unit.CommandTests
{
    public class JobOfferCommandTests
    {
        [Fact]
        public async Task GetJobOfferCommand_ReturnsOffers()
        {
            var command = new GetJobOfferCommand();

            var offers = new List<JobOfferModel>
            {
                new JobOfferModel { Id = 1 },
                new JobOfferModel { Id = 2 },
            };

            var repository = Substitute.For<IJobOfferRepository>();
            repository.GetAsync(command).Returns(offers);

            var handler = new GetJobOfferHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.Equal(res, offers);
        }

        [Fact]
        public async Task GetJobOfferByIdCommand_ReturnsOffer()
        {
            var command = new GetJobOfferByIdCommand(1);

            var offer = new JobOfferModel();

            var repository = Substitute.For<IJobOfferRepository>();
            repository.GetByIdAsync(command.Id).Returns(offer);

            var handler = new GetJobOfferByIdHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.Equal(offer, res);
        }

        [Fact]
        public async Task AddJobOfferCommand_AddsOffer()
        {
            var command = new AddJobOfferCommand();

            var offer = new JobOffer();

            var repository = Substitute.For<IJobOfferRepository>();
            repository.AddAsync(offer).Returns(Task.CompletedTask);

            var factory = Substitute.For<IJobOfferFactory>();
            factory.Create(command).Returns(offer);

            var validator = Substitute.For<IValidator<AddJobOfferRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new AddJobOfferHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            await repository.Received(1).AddAsync(offer);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task AddJobOfferCommand_InvalidRequest_Failure()
        {
            var command = new AddJobOfferCommand();

            var repository = Substitute.For<IJobOfferRepository>();

            var factory = Substitute.For<IJobOfferFactory>();

            var validator = Substitute.For<IValidator<AddJobOfferRequest>>();
            validator.Validate(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new AddJobOfferHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task UpdateJobOfferCommand_UpdatesOffer()
        {
            var command = new UpdateJobOfferCommand
            {
                Id = 1,
            };

            var offer = new JobOffer();

            var repository = Substitute.For<IJobOfferRepository>();
            repository.GetEntityByIdAsync(command.Id).Returns(offer);
            repository.UpdateAsync(offer).Returns(Task.CompletedTask);

            var factory = Substitute.For<IJobOfferFactory>();

            var validator = Substitute.For<IValidator<UpdateJobOfferRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new UpdateJobOfferHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            await repository.Received(1).UpdateAsync(offer);
            factory.Received(1).Update(offer, command);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task UpdateJobOfferCommand_InvalidCommand_Failure()
        {
            var command = new UpdateJobOfferCommand
            {
                Id = 1,
            };

            var repository = Substitute.For<IJobOfferRepository>();

            var factory = Substitute.For<IJobOfferFactory>();

            var validator = Substitute.For<IValidator<UpdateJobOfferRequest>>();
            validator.Validate(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new UpdateJobOfferHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task UpdateJobOfferCommand_EntityNotFound_Failure()
        {
            var command = new UpdateJobOfferCommand
            {
                Id = 1,
            };

            var repository = Substitute.For<IJobOfferRepository>();
            repository.GetEntityByIdAsync(command.Id).ReturnsNull();

            var factory = Substitute.For<IJobOfferFactory>();

            var validator = Substitute.For<IValidator<UpdateJobOfferRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new UpdateJobOfferHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task DeleteJobOfferByIdCommand_Success()
        {
            var command = new DeleteJobOfferByIdCommand(1);

            var repository = Substitute.For<IJobOfferRepository>();
            repository.DeleteByIdAsync(command.Id).Returns(Task.CompletedTask);

            var handler = new DeleteJobOfferByIdHandler(repository);

            var res = await handler.Handle(command, default);

            await repository.Received(1).DeleteByIdAsync(command.Id);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task DeleteJobOfferByIdCommand_ExceptionThrown_Failure()
        {
            var command = new DeleteJobOfferByIdCommand(1);

            const string ErrorMessage = "error";

            var repository = Substitute.For<IJobOfferRepository>();
            repository.DeleteByIdAsync(command.Id).Throws(new Exception(ErrorMessage));

            var handler = new DeleteJobOfferByIdHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.Equal(ErrorMessage, res.Error);
        }
    }
}

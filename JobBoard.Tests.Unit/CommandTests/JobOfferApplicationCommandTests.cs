using JobBoard.Application.Command;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Text;

namespace JobBoard.Tests.Unit.CommandTests
{
    public class JobOfferApplicationCommandTests
    {
        [Fact]
        public async Task AddJobOfferApplicationCommand_Success()
        {
            using var mockStream = new MemoryStream(Encoding.UTF8.GetBytes("mock"));

            var command = new AddJobOfferApplicationCommand
            {
                EmployeeId = 1,
                OfferId = 2,
                ResumeMimeType = "pdf",
                Resume = mockStream,
            };

            var createdEntity = new JobOfferApplication();
            const string FileName = "file";

            var repository = Substitute.For<IJobOfferApplicationRepository>();

            repository.AddAsync(createdEntity).Returns(Task.CompletedTask);

            var factory = Substitute.For<IJobOfferApplicationFactory>();
            factory.Create(command, FileName).Returns(createdEntity);

            var fileService = Substitute.For<IFileService>();
            fileService.SaveResumeFile(command.Resume).Returns(FileName);

            var handler = new AddJobOfferApplicationHandler(repository, factory, fileService);

            var res = await handler.Handle(command, default);

            await repository.Received(1).AddAsync(createdEntity);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task AddJobOfferApplication_InvalidResumeType_Failure()
        {
            var command = new AddJobOfferApplicationCommand
            {
                EmployeeId = 1,
                OfferId = 2,
                ResumeMimeType = "notpdf",
                Resume = null,
            };

            var repository = Substitute.For<IJobOfferApplicationRepository>();

            var factory = Substitute.For<IJobOfferApplicationFactory>();

            var fileService = Substitute.For<IFileService>();

            var handler = new AddJobOfferApplicationHandler(repository, factory, fileService);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
        }

        [Fact]
        public async Task DeleteJobOfferApplicationCommand_Success()
        {
            var command = new DeleteJobOfferApplicationByIdCommand(1);

            var application = new JobOfferApplication
            {
                ResumeFileName = "file"
            };

            var repository = Substitute.For<IJobOfferApplicationRepository>();
            repository.DeleteByIdAsync(command.Id).Returns(Task.CompletedTask);
            repository.GetEntityByIdAsync(command.Id).Returns(application);


            var fileService = Substitute.For<IFileService>();
            fileService.DeleteResumeFileAsync(application.ResumeFileName).Returns(Task.CompletedTask);

            var handler = new DeleteJobOfferApplicationByIdHandler(repository, fileService);

            var res = await handler.Handle(command, default);

            await fileService.Received(1).DeleteResumeFileAsync(application.ResumeFileName);
            await repository.Received(1).DeleteByIdAsync(command.Id);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task DeleteJobOfferApplicationByIdCommand_ApplicationNotFound_Success()
        {
            var command = new DeleteJobOfferApplicationByIdCommand(1);

            var repository = Substitute.For<IJobOfferApplicationRepository>();
            repository.GetEntityByIdAsync(command.Id).ReturnsNull();

            var fileService = Substitute.For<IFileService>();

            var handler = new DeleteJobOfferApplicationByIdHandler(repository, fileService);

            var res = await handler.Handle(command, default);

            Assert.True(res.IsSuccess);
        }
    }
}

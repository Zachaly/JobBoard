using JobBoard.Application.Factory;
using JobBoard.Domain.Entity;
using JobBoard.Domain.Enum;
using JobBoard.Model.JobOfferApplication;

namespace JobBoard.Tests.Unit.FactoryTests
{
    public class JobOfferApplicationFactoryTests
    {
        private readonly JobOfferApplicationFactory _factory;

        public JobOfferApplicationFactoryTests()
        {
            _factory = new JobOfferApplicationFactory();
        }

        [Fact]
        public void Create_CreatesProperEntity()
        {
            var request = new AddJobOfferApplicationRequest
            {
                EmployeeId = 1,
                OfferId = 2,
            };

            const string FileName = "file";

            var application = _factory.Create(request, FileName);

            Assert.Equal(FileName, application.ResumeFileName);
            Assert.Equal(request.EmployeeId, application.EmployeeId);
            Assert.Equal(request.OfferId, application.OfferId);
            Assert.NotEqual(default, application.ApplicationDate);
            Assert.Equal(JobOfferApplicationState.Sent, application.State);
        }

        [Fact]
        public void Update_UpdatesEntity()
        {
            var entity = new JobOfferApplication
            {
                State = JobOfferApplicationState.Sent,
            };

            var request = new UpdateJobOfferApplicationRequest
            {
                State = JobOfferApplicationState.Accepted
            };

            _factory.Update(entity, request);

            Assert.Equal(request.State, entity.State);
        }
    }
}

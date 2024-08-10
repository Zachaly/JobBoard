using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOfferApplication;

namespace JobBoard.Application.Command
{
    public record GetJobOfferApplicationByIdCommand(long Id) : GetByIdCommand<JobOfferApplicationModel>(Id);

    public class GetJobOfferApplicationByIdHandler : GetByIdHandler<JobOfferApplicationModel, GetJobOfferApplicationRequest,
        GetJobOfferApplicationByIdCommand>
    {
        public GetJobOfferApplicationByIdHandler(IJobOfferApplicationRepository repository) : base(repository)
        {
        }
    }
}

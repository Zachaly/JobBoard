using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOfferApplication;

namespace JobBoard.Application.Command
{
    public class GetJobOfferApplicationCountCommand : GetJobOfferApplicationRequest, IGetCountCommand
    {
    }

    public class GetJobOfferApplicationCountHandler : GetCountHandler<JobOfferApplicationModel, GetJobOfferApplicationRequest,
        GetJobOfferApplicationCountCommand>
    {
        public GetJobOfferApplicationCountHandler(IJobOfferApplicationRepository repository) : base(repository)
        {
        }
    }
}

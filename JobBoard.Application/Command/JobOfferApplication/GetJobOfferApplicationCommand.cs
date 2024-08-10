using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOfferApplication;

namespace JobBoard.Application.Command
{
    public class GetJobOfferApplicationCommand : GetJobOfferApplicationRequest, IGetEntityCommand<JobOfferApplicationModel>
    {
    }

    public class GetJobOfferApplicationHandler : GetEntityHandler<JobOfferApplicationModel, GetJobOfferApplicationRequest,
        GetJobOfferApplicationCommand>
    {
        public GetJobOfferApplicationHandler(IJobOfferApplicationRepository repository) : base(repository)
        {
        }
    }
}

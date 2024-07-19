using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOfferRequirement;

namespace JobBoard.Application.Command
{
    public class GetJobOfferRequirementCountCommand : GetJobOfferRequirementRequest, IGetCountCommand
    {
    }

    public class GetJobOfferRequirementCountHandler : GetCountHandler<JobOfferRequirementModel, GetJobOfferRequirementRequest,
        GetJobOfferRequirementCountCommand>
    {
        public GetJobOfferRequirementCountHandler(IJobOfferRequirementRepository repository) : base(repository)
        {
        }
    }
}

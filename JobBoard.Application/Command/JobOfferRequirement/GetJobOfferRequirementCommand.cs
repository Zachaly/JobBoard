using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOfferRequirement;

namespace JobBoard.Application.Command
{
    public class GetJobOfferRequirementCommand : GetJobOfferRequirementRequest, IGetEntityCommand<JobOfferRequirementModel>
    {
    }

    public class GetJobOfferRequirementHandler : GetEntityHandler<JobOfferRequirementModel, GetJobOfferRequirementRequest,
        GetJobOfferRequirementCommand>
    {
        public GetJobOfferRequirementHandler(IJobOfferRequirementRepository repository) : base(repository)
        {
        }
    }
}

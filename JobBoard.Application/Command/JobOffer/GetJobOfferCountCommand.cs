using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOffer;

namespace JobBoard.Application.Command
{
    public class GetJobOfferCountCommand : GetJobOfferRequest, IGetCountCommand
    {
    }

    public class GetJobOfferCountHandler : GetCountHandler<JobOfferModel, GetJobOfferRequest, GetJobOfferCountCommand>
    {
        public GetJobOfferCountHandler(IJobOfferRepository repository) : base(repository)
        {
            
        }
    }
}

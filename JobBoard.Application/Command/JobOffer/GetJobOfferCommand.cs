using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOffer;
using MediatR;

namespace JobBoard.Application.Command
{
    public class GetJobOfferCommand : GetJobOfferRequest, IGetEntityCommand<JobOfferModel>
    {
    }

    public class GetJobOfferHandler : GetEntityHandler<JobOfferModel, GetJobOfferRequest, GetJobOfferCommand>
    {
        public GetJobOfferHandler(IJobOfferRepository repository) : base(repository)
        {
        }
    }
}

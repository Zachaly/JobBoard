using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOffer;
using MediatR;

namespace JobBoard.Application.Command
{
    public record GetJobOfferByIdCommand(long Id) : GetByIdCommand<JobOfferModel>(Id);

    public class GetJobOfferByIdHandler : GetByIdHandler<JobOfferModel, GetJobOfferRequest, GetJobOfferByIdCommand>
    {
        public GetJobOfferByIdHandler(IJobOfferRepository repository) : base(repository)
        {
        }
    }
}

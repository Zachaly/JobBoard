using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOffer;
using MediatR;

namespace JobBoard.Application.Command
{
    public class GetJobOfferCommand : GetJobOfferRequest, IRequest<IEnumerable<JobOfferModel>>
    {
    }

    public class GetJobOfferHandler : IRequestHandler<GetJobOfferCommand, IEnumerable<JobOfferModel>>
    {
        private readonly IJobOfferRepository _repository;

        public GetJobOfferHandler(IJobOfferRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<JobOfferModel>> Handle(GetJobOfferCommand request, CancellationToken cancellationToken)
            => _repository.GetAsync(request);
    }
}

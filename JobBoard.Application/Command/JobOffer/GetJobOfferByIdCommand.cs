using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOffer;
using MediatR;

namespace JobBoard.Application.Command
{
    public record GetJobOfferByIdCommand(long Id) : IRequest<JobOfferModel?>;

    public class GetJobOfferByIdHandler : IRequestHandler<GetJobOfferByIdCommand, JobOfferModel?>
    {
        private readonly IJobOfferRepository _repository;

        public GetJobOfferByIdHandler(IJobOfferRepository repository)
        {
            _repository = repository;
        }

        public Task<JobOfferModel?> Handle(GetJobOfferByIdCommand request, CancellationToken cancellationToken)
            => _repository.GetByIdAsync(request.Id);
    }
}

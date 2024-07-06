using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOfferRequirement;
using MediatR;

namespace JobBoard.Application.Command
{
    public class GetJobOfferRequirementCommand : GetJobOfferRequirementRequest, IRequest<IEnumerable<JobOfferRequirementModel>>
    {
    }

    public class GetJobOfferRequirementHandler : IRequestHandler<GetJobOfferRequirementCommand, IEnumerable<JobOfferRequirementModel>>
    {
        private readonly IJobOfferRequirementRepository _repository;

        public GetJobOfferRequirementHandler(IJobOfferRequirementRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<JobOfferRequirementModel>> Handle(GetJobOfferRequirementCommand request, CancellationToken cancellationToken)
            => _repository.GetAsync(request);
    }
}

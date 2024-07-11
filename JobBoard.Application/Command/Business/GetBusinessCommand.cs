using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.Business;
using MediatR;

namespace JobBoard.Application.Command
{
    public class GetBusinessCommand : GetBusinessRequest, IRequest<IEnumerable<BusinessModel>>
    {
    }

    public class GetBusinessHandler : IRequestHandler<GetBusinessCommand, IEnumerable<BusinessModel>>
    {
        private readonly IBusinessRepository _repository;

        public GetBusinessHandler(IBusinessRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<BusinessModel>> Handle(GetBusinessCommand request, CancellationToken cancellationToken)
            => _repository.GetAsync(request);
    }
}

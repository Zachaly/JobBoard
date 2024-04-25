using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.CompanyAccount;
using MediatR;

namespace JobBoard.Application.Command
{
    public class GetCompanyAccountCommand : GetCompanyRequest, IRequest<IEnumerable<CompanyModel>>
    {
    }

    public class GetCompanyAccountHandler : IRequestHandler<GetCompanyAccountCommand, IEnumerable<CompanyModel>>
    {
        private readonly ICompanyAccountRepository _repository;

        public GetCompanyAccountHandler(ICompanyAccountRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<CompanyModel>> Handle(GetCompanyAccountCommand request, CancellationToken cancellationToken)
            => _repository.GetAsync(request);
    }
}

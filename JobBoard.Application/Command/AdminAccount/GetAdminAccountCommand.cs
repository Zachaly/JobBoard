using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.AdminAccount;
using MediatR;

namespace JobBoard.Application.Command
{
    public class GetAdminAccountCommand : GetAdminAccountRequest, IRequest<IEnumerable<AdminAccountModel>>
    {
    }

    public class GetAdminAccountHandler : IRequestHandler<GetAdminAccountCommand, IEnumerable<AdminAccountModel>>
    {
        private readonly IAdminAccountRepository _repository;

        public GetAdminAccountHandler(IAdminAccountRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<AdminAccountModel>> Handle(GetAdminAccountCommand request, CancellationToken cancellationToken)
            => _repository.GetAsync(request);
    }
}

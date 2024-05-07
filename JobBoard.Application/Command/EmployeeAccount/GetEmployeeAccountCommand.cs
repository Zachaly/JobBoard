using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.EmployeeAccount;
using MediatR;

namespace JobBoard.Application.Command
{
    public class GetEmployeeAccountCommand : GetEmployeeAccountRequest, IRequest<IEnumerable<EmployeeAccountModel>>
    {
    }

    public class GetEmployeeAccountHandler : IRequestHandler<GetEmployeeAccountCommand, IEnumerable<EmployeeAccountModel>>
    {
        private readonly IEmployeeAccountRepository _repository;

        public GetEmployeeAccountHandler(IEmployeeAccountRepository repository)
        {
            _repository = repository;   
        }

        public Task<IEnumerable<EmployeeAccountModel>> Handle(GetEmployeeAccountCommand request, CancellationToken cancellationToken)
            => _repository.GetAsync(request);
    }
}

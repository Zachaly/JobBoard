using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.EmployeeAccount;
using MediatR;

namespace JobBoard.Application.Command
{
    public record GetEmployeeAccountByIdCommand(long Id) : IRequest<EmployeeAccountModel?>
    {
    }

    public class GetEmployeeAccountByIdHandler : IRequestHandler<GetEmployeeAccountByIdCommand, EmployeeAccountModel?>
    {
        private readonly IEmployeeAccountRepository _repository;

        public GetEmployeeAccountByIdHandler(IEmployeeAccountRepository repository)
        {
            _repository = repository;
        }

        public Task<EmployeeAccountModel?> Handle(GetEmployeeAccountByIdCommand request, CancellationToken cancellationToken)
            => _repository.GetByIdAsync(request.Id);
    }
}

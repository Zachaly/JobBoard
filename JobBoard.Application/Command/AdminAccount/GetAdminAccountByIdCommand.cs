using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.AdminAccount;
using MediatR;

namespace JobBoard.Application.Command
{
    public record GetAdminAccountByIdCommand(long Id) : IRequest<AdminAccountModel?> { }

    public class GetAdminAccountByIdHandler : IRequestHandler<GetAdminAccountByIdCommand, AdminAccountModel?>
    {
        private readonly IAdminAccountRepository _repository;

        public GetAdminAccountByIdHandler(IAdminAccountRepository repository)
        {
            _repository = repository;
        }

        public Task<AdminAccountModel?> Handle(GetAdminAccountByIdCommand request, CancellationToken cancellationToken)
            => _repository.GetByIdAsync(request.Id);
    }
}

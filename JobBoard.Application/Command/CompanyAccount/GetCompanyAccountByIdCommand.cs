using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.CompanyAccount;
using MediatR;

namespace JobBoard.Application.Command
{
    public record GetCompanyAccountByIdCommand(long Id) : IRequest<CompanyModel?>
    {
    }

    public class GetCompanyAccountByIdHandler : IRequestHandler<GetCompanyAccountByIdCommand, CompanyModel?>
    {
        private readonly ICompanyAccountRepository _repository;

        public GetCompanyAccountByIdHandler(ICompanyAccountRepository repository)
        {
            _repository = repository;
        }

        public Task<CompanyModel?> Handle(GetCompanyAccountByIdCommand request, CancellationToken cancellationToken)
            => _repository.GetByIdAsync(request.Id);
    }
}

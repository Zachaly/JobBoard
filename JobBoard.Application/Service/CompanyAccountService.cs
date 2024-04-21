using FluentValidation;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.CompanyAccount;
using JobBoard.Model.Response;

namespace JobBoard.Application.Service
{
    public class CompanyAccountService : ICompanyAccountService
    {
        private readonly ICompanyAccountRepository _repository;
        private readonly ICompanyAccountFactory _factory;
        private readonly IValidator<AddCompanyAccountRequest> _addRequestValidator;
        private readonly IHashService _hashService;

        public CompanyAccountService(ICompanyAccountRepository repository, ICompanyAccountFactory factory,
            IValidator<AddCompanyAccountRequest> addRequestValidator, IHashService hashService) 
        {
            _repository = repository;
            _factory = factory;
            _addRequestValidator = addRequestValidator;
            _hashService = hashService;
        }

        public async Task<ResponseModel> AddAsync(AddCompanyAccountRequest request)
        {
            var validation = _addRequestValidator.Validate(request);

            if(!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var existingAccount = await _repository.GetByEmailAsync(request.Email);

            if(existingAccount is not null)
            {
                return new ResponseModel("Email already taken");
            }

            var passwordHash = _hashService.HashPassword(request.Password);

            var account = _factory.Create(request, passwordHash);

            await _repository.AddAsync(account);

            return new ResponseModel();
        }

        public Task<IEnumerable<CompanyModel>> GetAsync(GetCompanyRequest request)
        {
            return _repository.GetAsync(request);
        }

        public Task<CompanyModel?> GetById(long id)
        {
            return _repository.GetByIdAsync(id);
        }
    }
}

using FluentValidation;
using FluentValidation.Results;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.CompanyAccount;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace JobBoard.Tests.Unit.Service
{
    public class CompanyAccountServiceTests
    {
        private readonly ICompanyAccountRepository _repository;
        private readonly ICompanyAccountFactory _factory;
        private readonly IHashService _hashService;
        private readonly IValidator<AddCompanyAccountRequest> _validator;
        private readonly CompanyAccountService _service;

        public CompanyAccountServiceTests()
        {
            _repository = Substitute.For<ICompanyAccountRepository>();
            _factory = Substitute.For<ICompanyAccountFactory>();
            _hashService = Substitute.For<IHashService>();
            _validator = Substitute.For<IValidator<AddCompanyAccountRequest>>();

            _service = new CompanyAccountService(_repository, _factory, _validator, _hashService);
        }

        [Fact]
        public async Task AddAsync_ValidRequest_AddsAccount()
        {
            var request = new AddCompanyAccountRequest("email", "pass", "name", "city", "postal", "addr", "ctn", "em");

            var createdAccount = new CompanyAccount();

            const string GeneratedHash = "hash";

            _repository.GetByEmailAsync(request.Email).ReturnsNull();

            _repository.AddAsync(createdAccount).Returns(Task.CompletedTask);

            _hashService.HashPassword(request.Password).Returns(GeneratedHash);

            _factory.Create(request, GeneratedHash).Returns(createdAccount);

            _validator.Validate(request).Returns(new ValidationResult());

            var res = await _service.AddAsync(request);

            await _repository.Received(1).AddAsync(createdAccount);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task AddAsync_InvalidRequest_Failure()
        {
            var request = new AddCompanyAccountRequest("email", "pass", "name", "city", "postal", "addr", "ctn", "em");

            _validator.Validate(request).Returns(new ValidationResult(new List<ValidationFailure>()
            {
                new ValidationFailure("prop", "err")
            }));

            var res = await _service.AddAsync(request);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors!);
        }

        [Fact]
        public async Task AddAsync_ValidRequest_EmailTaken_Failure()
        {
            var request = new AddCompanyAccountRequest("email", "pass", "name", "city", "postal", "addr", "ctn", "em");

            _repository.GetByEmailAsync(request.Email).Returns(new CompanyModel());

            _validator.Validate(request).Returns(new ValidationResult());

            var res = await _service.AddAsync(request);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task GetAsync_ReturnsAccounts()
        {
            var accounts = new List<CompanyModel>()
            {
                new CompanyModel(), new CompanyModel(), new CompanyModel()
            };

            var request = new GetCompanyRequest();

            _repository.GetAsync(request).Returns(accounts);

            var res = await _service.GetAsync(request);

            Assert.Equal(accounts, res);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsSpecifiedAccount()
        {
            const long Id = 1;

            var model = new CompanyModel();

            _repository.GetByIdAsync(Id).Returns(model);

            var res = await _service.GetById(Id);

            Assert.Equal(model, res);
        }
    }
}

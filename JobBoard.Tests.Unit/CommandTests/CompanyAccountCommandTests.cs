using FluentValidation;
using FluentValidation.Results;
using JobBoard.Application.Command;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Application.Service.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.CompanyAccount;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Text;

namespace JobBoard.Tests.Unit.CommandTests
{
    public class CompanyAccountCommandTests
    {

        [Fact]
        public async Task AddCompanyAccountCommand_ValidRequest_AddsAccount()
        {
            var request = new AddCompanyAccountCommand();

            var repository = Substitute.For<ICompanyAccountRepository>();

            var hashService = Substitute.For<IHashService>();

            var factory = Substitute.For<ICompanyAccountFactory>();

            var validator = Substitute.For<IValidator<AddCompanyAccountRequest>>();

            var createdAccount = new CompanyAccount();

            const string GeneratedHash = "hash";

            repository.GetByEmailAsync(request.Email).ReturnsNull();

            repository.AddAsync(createdAccount).Returns(Task.CompletedTask);

            hashService.HashPassword(request.Password).Returns(GeneratedHash);

            factory.Create(request, GeneratedHash).Returns(createdAccount);

            validator.Validate(request).Returns(new ValidationResult());

            var handler = new AddCompanyAccountHandler(repository, factory, validator, hashService);

            var res = await handler.Handle(request, default);

            await repository.Received(1).AddAsync(createdAccount);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task AddCompanyAccountCommand_InvalidRequest_Failure()
        {
            var request = new AddCompanyAccountCommand();

            var repository = Substitute.For<ICompanyAccountRepository>();

            var hashService = Substitute.For<IHashService>();

            var factory = Substitute.For<ICompanyAccountFactory>();

            var validator = Substitute.For<IValidator<AddCompanyAccountRequest>>();

            validator.Validate(request).Returns(new ValidationResult(new List<ValidationFailure>()
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new AddCompanyAccountHandler(repository, factory, validator, hashService);

            var res = await handler.Handle(request, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors!);
        }

        [Fact]
        public async Task AddCompanyAccountCommand_ValidRequest_EmailTaken_Failure()
        {
            var request = new AddCompanyAccountCommand();

            var repository = Substitute.For<ICompanyAccountRepository>();

            var hashService = Substitute.For<IHashService>();

            var factory = Substitute.For<ICompanyAccountFactory>();

            var validator = Substitute.For<IValidator<AddCompanyAccountRequest>>();

            repository.GetByEmailAsync(request.Email).Returns(new CompanyAccount());

            validator.Validate(request).Returns(new ValidationResult());

            var handler = new AddCompanyAccountHandler(repository, factory, validator, hashService);

            var res = await handler.Handle(request, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task GetCompanyAccountCommand_ReturnsAccounts()
        {
            var accounts = new List<CompanyModel>()
            {
                new CompanyModel(), new CompanyModel(), new CompanyModel()
            };

            var request = new GetCompanyAccountCommand();

            var repository = Substitute.For<ICompanyAccountRepository>();

            repository.GetAsync(request).Returns(accounts);

            var handler = new GetCompanyAccountHandler(repository);

            var res = await handler.Handle(request, default);

            Assert.Equal(accounts, res);
        }

        [Fact]
        public async Task GetCompanyAccountByIdCommand_ReturnsSpecifiedAccount()
        {
            const long Id = 1;

            var model = new CompanyModel();

            var repository = Substitute.For<ICompanyAccountRepository>();

            repository.GetByIdAsync(Id).Returns(model);

            var handler = new GetCompanyAccountByIdHandler(repository);

            var res = await handler.Handle(new GetCompanyAccountByIdCommand(Id), default);

            Assert.Equal(model, res);
        }

        [Fact]
        public async Task CompanyLoginCommand_ReturnsResponseWithTokenAndId()
        {
            var request = new CompanyLoginCommand();

            var account = new CompanyAccount
            {
                Id = 1,
            };

            const string RefreshToken = "reftoken";
            const string Token = "token";

            var repository = Substitute.For<ICompanyAccountRepository>();
            repository.GetByEmailAsync(request.Login).Returns(account);

            var hashService = Substitute.For<IHashService>();
            hashService.VerifyPassword(request.Password, account.PasswordHash).Returns(true);

            var tokenService = Substitute.For<IAccessTokenService>();
            tokenService.GenerateTokenAsync(account.Id, "Company").Returns(Token);

            var refreshTokenService = Substitute.For<IRefreshTokenService>();
            refreshTokenService.GenerateCompanyAccountTokenAsync(account.Id).Returns(RefreshToken);

            var handler = new CompanyLoginHandler(repository, hashService, tokenService, refreshTokenService);

            var res = await handler.Handle(request, default);

            Assert.True(res.IsSuccess);
            Assert.Equal(account.Id, res.UserId);
            Assert.Equal(Token, res.AuthToken);
            Assert.Equal(RefreshToken, res.RefreshToken);
        }

        [Fact]
        public async Task CompanyLoginCommand_UserNotFound_Failure()
        {
            var request = new CompanyLoginCommand();

            var repository = Substitute.For<ICompanyAccountRepository>();
            repository.GetByEmailAsync(request.Login).ReturnsNull();

            var hashService = Substitute.For<IHashService>();

            var tokenService = Substitute.For<IAccessTokenService>();

            var refreshTokenService = Substitute.For<IRefreshTokenService>();

            var handler = new CompanyLoginHandler(repository, hashService, tokenService, refreshTokenService);

            var res = await handler.Handle(request, default);

            Assert.False(res.IsSuccess);
            Assert.Equal(0, res.UserId);
            Assert.Empty(res.AuthToken);
            Assert.Empty(res.RefreshToken);
        }

        [Fact]
        public async Task CompanyLoginCommand_PasswordDoNotMatch_Failure()
        {
            var request = new CompanyLoginCommand();

            var account = new CompanyAccount
            {
                Id = 1,
            };

            var repository = Substitute.For<ICompanyAccountRepository>();
            repository.GetByEmailAsync(request.Login).Returns(account);

            var hashService = Substitute.For<IHashService>();
            hashService.VerifyPassword(request.Password, account.PasswordHash).Returns(false);

            var tokenService = Substitute.For<IAccessTokenService>();

            var refreshTokenService = Substitute.For<IRefreshTokenService>();

            var handler = new CompanyLoginHandler(repository, hashService, tokenService, refreshTokenService);

            var res = await handler.Handle(request, default);

            Assert.False(res.IsSuccess);
            Assert.Equal(0, res.UserId);
            Assert.Empty(res.AuthToken);
            Assert.Empty(res.RefreshToken);
        }

        [Fact]
        public async Task UpdateCompanyAccountCommand_UpdatesEntity()
        {
            var command = new UpdateCompanyAccountCommand
            {
                Id = 1,
                Address = "addr",
                City = "ci",
                ContactEmail = "email@email.com",
                Country = "ctn",
                Name = "namee",
                PostalCode = "postal"
            };

            var account = new CompanyAccount();

            var repository = Substitute.For<ICompanyAccountRepository>();
            repository.UpdateAsync(account).Returns(Task.CompletedTask);
            repository.GetEntityByIdAsync(command.Id).Returns(account);

            var factory = Substitute.For<ICompanyAccountFactory>();

            var validator = Substitute.For<IValidator<UpdateCompanyAccountRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new UpdateCompanyAccountHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            await repository.Received(1).UpdateAsync(account);
            factory.Received(1).Update(account, command);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task UpdateCompanyAccountCommand_InvalidCommand_Failure()
        {
            var command = new UpdateCompanyAccountCommand();

            var repository = Substitute.For<ICompanyAccountRepository>();

            var factory = Substitute.For<ICompanyAccountFactory>();

            var validator = Substitute.For<IValidator<UpdateCompanyAccountRequest>>();
            validator.Validate(command).Returns(new ValidationResult(new List<ValidationFailure>()
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new UpdateCompanyAccountHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task UpdateCompanyAccountCommand_EntityNotFound_Failure()
        {
            var command = new UpdateCompanyAccountCommand();

            var repository = Substitute.For<ICompanyAccountRepository>();
            repository.GetEntityByIdAsync(command.Id).ReturnsNull();

            var factory = Substitute.For<ICompanyAccountFactory>();

            var validator = Substitute.For<IValidator<UpdateCompanyAccountRequest>>();
            validator.Validate(command).Returns(new ValidationResult());

            var handler = new UpdateCompanyAccountHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task UpdateCompanyAccountPictureCommand_PictureSend_Success()
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("mock"));

            var command = new UpdateCompanyAccountProfilePictureCommand(1, stream, "jpg");

            var company = new CompanyAccount();
            const string FileName = "filename";

            var fileService = Substitute.For<IFileService>();

            fileService.SaveCompanyProfilePictureAsync(command.Picture).Returns(FileName);
            fileService.DeleteCompanyProfilePictureAsync(null).Returns(Task.CompletedTask);

            var repository = Substitute.For<ICompanyAccountRepository>();
            repository.GetEntityByIdAsync(command.CompanyId).Returns(company);
            repository.UpdateAsync(company).Returns(Task.CompletedTask);

            var handler = new UpdateCompanyAccountProfilePictureHandler(repository, fileService);

            var res = await handler.Handle(command, default);

            await repository.Received(1).UpdateAsync(company);
            await fileService.Received(1).DeleteCompanyProfilePictureAsync(null);

            Assert.True(res.IsSuccess);
            Assert.Equal(FileName, company.ProfilePicture);
        }

        [Fact]
        public async Task UpdateCompanyAccountPictureCommand_PictureSend_InvalidMimeType_Failure()
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("mock"));

            var command = new UpdateCompanyAccountProfilePictureCommand(1, stream, "mp3");

            var company = new CompanyAccount();
            const string FileName = "filename";

            var fileService = Substitute.For<IFileService>();
            var repository = Substitute.For<ICompanyAccountRepository>();

            repository.GetEntityByIdAsync(command.CompanyId).Returns(company);

            var handler = new UpdateCompanyAccountProfilePictureHandler(repository, fileService);

            var res = await handler.Handle(command, default);

            await repository.Received(0).UpdateAsync(company);
            await fileService.Received(0).DeleteCompanyProfilePictureAsync(null);

            Assert.False(res.IsSuccess);
        }

        [Fact]
        public async Task UpdateCompanyAccountPictureCommand_NoPictureSend_Success()
        {
            var command = new UpdateCompanyAccountProfilePictureCommand(1, null, null);

            const string FileName = "filename";
            var company = new CompanyAccount() { ProfilePicture = FileName };

            var fileService = Substitute.For<IFileService>();

            fileService.DeleteCompanyProfilePictureAsync(FileName).Returns(Task.CompletedTask);

            var repository = Substitute.For<ICompanyAccountRepository>();
            repository.GetEntityByIdAsync(command.CompanyId).Returns(company);
            repository.UpdateAsync(company).Returns(Task.CompletedTask);

            var handler = new UpdateCompanyAccountProfilePictureHandler(repository, fileService);

            var res = await handler.Handle(command, default);

            await repository.Received(1).UpdateAsync(company);
            await fileService.Received(1).DeleteCompanyProfilePictureAsync(FileName);

            Assert.True(res.IsSuccess);
            Assert.Null(company.ProfilePicture);
        }

        [Fact]
        public async Task UpdateCompanyAccountPictureCommand_EntityNotFound_Failure()
        {
            var command = new UpdateCompanyAccountProfilePictureCommand(1, null, "jpg");

            var fileService = Substitute.For<IFileService>();

            var repository = Substitute.For<ICompanyAccountRepository>();
            repository.GetEntityByIdAsync(command.CompanyId).ReturnsNull();

            var handler = new UpdateCompanyAccountProfilePictureHandler(repository, fileService);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
        }
    }
}

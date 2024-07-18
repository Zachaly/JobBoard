using FluentValidation;
using JobBoard.Application.Command.Abstraction;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.Business;
using JobBoard.Model.Response;

namespace JobBoard.Application.Command
{
    public class AddBusinessCommand : AddBusinessRequest, IAddEntityCommand
    {
    }

    public class AddBusinessHandler : AddEntityHandler<Business, AddBusinessRequest, AddBusinessCommand>
    {
        private readonly IBusinessRepository _repository;

        public AddBusinessHandler(IBusinessRepository repository, IBusinessFactory factory, IValidator<AddBusinessRequest> validator)
            : base(repository, factory, validator)
        {
            _repository = repository;
        }

        public override async Task<ResponseModel> Handle(AddBusinessCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _repository.GetByNameAsync(request.Name);

            if (existingEntity is not null)
            {
                return new ResponseModel("Name already taken");
            }

            return await base.Handle(request, cancellationToken);
        }
    }
}

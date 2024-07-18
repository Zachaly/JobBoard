using FluentValidation;
using JobBoard.Application.Command.Abstraction;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.Business;
using JobBoard.Model.Response;

namespace JobBoard.Application.Command
{
    public class UpdateBusinessCommand : UpdateBusinessRequest, IUpdateEntityCommand
    {
    }

    public class UpdateBusinessHandler : UpdateEntityHandler<Business, UpdateBusinessRequest, UpdateBusinessCommand>
    {
        private readonly IBusinessRepository _repository;

        public UpdateBusinessHandler(IBusinessRepository repository, IBusinessFactory factory,
            IValidator<UpdateBusinessRequest> validator) : base(repository, factory, validator)
        {
            _repository = repository;
        }

        public override async Task<ResponseModel> Handle(UpdateBusinessCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _repository.GetByNameAsync(request.Name);

            if(existingEntity is not null)
            {
                return new ResponseModel("Name already taken");
            }

            return await base.Handle(request, cancellationToken);
        }
    }
}

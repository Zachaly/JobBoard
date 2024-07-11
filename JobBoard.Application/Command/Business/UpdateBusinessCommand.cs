using FluentValidation;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.Business;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public class UpdateBusinessCommand : UpdateBusinessRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateBusinessHandler : IRequestHandler<UpdateBusinessCommand, ResponseModel>
    {
        private readonly IBusinessRepository _repository;
        private readonly IBusinessFactory _factory;
        private readonly IValidator<UpdateBusinessRequest> _validator;

        public UpdateBusinessHandler(IBusinessRepository repository, IBusinessFactory factory,
            IValidator<UpdateBusinessRequest> validator)
        {
            _repository = repository;
            _factory = factory;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(UpdateBusinessCommand request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if(!validation.IsValid) 
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var existingBusiness = await _repository.GetByNameAsync(request.Name);

            if(existingBusiness is not null)
            {
                return new ResponseModel("Name already taken");
            }

            var business = await _repository.GetEntityByIdAsync(request.Id);

            if(business is null)
            {
                return new ResponseModel("Entity not found");
            }

            _factory.Update(business, request);

            await _repository.UpdateAsync(business);

            return new ResponseModel();
        }
    }
}

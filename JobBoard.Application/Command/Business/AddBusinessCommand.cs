using FluentValidation;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.Business;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public class AddBusinessCommand : AddBusinessRequest, IRequest<ResponseModel>
    {
    }

    public class AddBusinessHandler : IRequestHandler<AddBusinessCommand, ResponseModel>
    {
        private readonly IBusinessRepository _repository;
        private readonly IBusinessFactory _factory;
        private readonly IValidator<AddBusinessRequest> _validator;

        public AddBusinessHandler(IBusinessRepository repository, IBusinessFactory factory, IValidator<AddBusinessRequest> validator)
        {
            _repository = repository;
            _factory = factory;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(AddBusinessCommand request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if(!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var existingEntity = await _repository.GetByNameAsync(request.Name);

            if(existingEntity is not null)
            {
                return new ResponseModel("Name already taken");
            }

            var business = _factory.Create(request);

            await _repository.AddAsync(business);

            return new ResponseModel();
        }
    }
}

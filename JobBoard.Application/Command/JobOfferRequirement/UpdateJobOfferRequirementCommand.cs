using FluentValidation;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOfferRequirement;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public class UpdateJobOfferRequirementCommand : UpdateJobOfferRequirementRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateJobOfferRequirementHandler : IRequestHandler<UpdateJobOfferRequirementCommand, ResponseModel>
    {
        private readonly IJobOfferRequirementRepository _repository;
        private readonly IJobOfferRequirementFactory _factory;
        private readonly IValidator<UpdateJobOfferRequirementRequest> _validator;

        public UpdateJobOfferRequirementHandler(IJobOfferRequirementRepository repository, IJobOfferRequirementFactory factory,
            IValidator<UpdateJobOfferRequirementRequest> validator)
        {
            _repository = repository;
            _factory = factory;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(UpdateJobOfferRequirementCommand request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if(!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var entity = await _repository.GetEntityByIdAsync(request.Id);

            if(entity is null)
            {
                return new ResponseModel("Entity not found");
            }

            _factory.Update(entity, request);

            await _repository.UpdateAsync(entity);

            return new ResponseModel();
        }
    }
}

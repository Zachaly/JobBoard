using FluentValidation;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOfferRequirement;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public class AddJobOfferRequirementCommand : AddJobOfferRequirementRequest, IRequest<ResponseModel>
    {
    }

    public class AddJobOfferRequirementHandler : IRequestHandler<AddJobOfferRequirementCommand, ResponseModel>
    {
        private readonly IJobOfferRequirementRepository _repository;
        private readonly IJobOfferRequirementFactory _factory;
        private readonly IValidator<AddJobOfferRequirementRequest> _validator;

        public AddJobOfferRequirementHandler(IJobOfferRequirementRepository repository, IJobOfferRequirementFactory factory,
            IValidator<AddJobOfferRequirementRequest> validator)
        {
            _repository = repository;
            _factory = factory;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(AddJobOfferRequirementCommand request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if(!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var requirement = _factory.Create(request);

            await _repository.AddAsync(requirement);

            return new ResponseModel();
        }
    }
}

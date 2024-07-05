using FluentValidation;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOffer;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public class UpdateJobOfferCommand : UpdateJobOfferRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateJobOfferHandler : IRequestHandler<UpdateJobOfferCommand, ResponseModel>
    {
        private readonly IJobOfferRepository _repository;
        private readonly IJobOfferFactory _factory;
        private readonly IValidator<UpdateJobOfferRequest> _validator;

        public UpdateJobOfferHandler(IJobOfferRepository repository, IJobOfferFactory factory,
            IValidator<UpdateJobOfferRequest> validator)
        {
            _repository = repository;
            _factory = factory;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(UpdateJobOfferCommand request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if(!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var offer = await _repository.GetEntityByIdAsync(request.Id);

            if(offer is null)
            {
                return new ResponseModel("Entity not found");
            }

            _factory.Update(offer, request);

            await _repository.UpdateAsync(offer);

            return new ResponseModel();
        }
    }
}

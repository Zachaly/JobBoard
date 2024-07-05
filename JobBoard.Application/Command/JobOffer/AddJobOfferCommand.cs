using FluentValidation;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.JobOffer;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public class AddJobOfferCommand : AddJobOfferRequest, IRequest<ResponseModel>
    {
    }

    public class AddJobOfferHandler : IRequestHandler<AddJobOfferCommand, ResponseModel>
    {
        private readonly IJobOfferRepository _repository;
        private readonly IJobOfferFactory _factory;
        private readonly IValidator<AddJobOfferRequest> _validator;

        public AddJobOfferHandler(IJobOfferRepository repository, IJobOfferFactory factory,
            IValidator<AddJobOfferRequest> validator)
        {
            _repository = repository;
            _factory = factory;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(AddJobOfferCommand request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if (!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var offer = _factory.Create(request);

            await _repository.AddAsync(offer);

            return new ResponseModel();
        }
    }
}

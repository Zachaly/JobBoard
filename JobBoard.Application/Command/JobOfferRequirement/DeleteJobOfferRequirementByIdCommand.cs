using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public record DeleteJobOfferRequirementByIdCommand(long Id) : IRequest<ResponseModel>;

    public class DeleteJobOfferRequirementByIdHandler : IRequestHandler<DeleteJobOfferRequirementByIdCommand, ResponseModel>
    {
        private readonly IJobOfferRequirementRepository _repository;

        public DeleteJobOfferRequirementByIdHandler(IJobOfferRequirementRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseModel> Handle(DeleteJobOfferRequirementByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.DeleteByIdAsync(request.Id);

                return new ResponseModel();
            }
            catch(System.Exception ex)
            {
                return new ResponseModel(ex.Message);
            }
        }
    }
}

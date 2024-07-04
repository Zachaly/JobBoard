using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public record DeleteJobOfferByIdCommand(long Id) : IRequest<ResponseModel>;

    public class DeleteJobOfferByIdHandler : IRequestHandler<DeleteJobOfferByIdCommand, ResponseModel>
    {
        private readonly IJobOfferRepository _repository;

        public DeleteJobOfferByIdHandler(IJobOfferRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseModel> Handle(DeleteJobOfferByIdCommand request, CancellationToken cancellationToken)
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

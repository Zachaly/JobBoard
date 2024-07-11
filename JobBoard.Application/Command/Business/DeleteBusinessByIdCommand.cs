using JobBoard.Database.Repository.Abstraction;
using JobBoard.Model.Response;
using MediatR;

namespace JobBoard.Application.Command
{
    public record DeleteBusinessByIdCommand(long Id) : IRequest<ResponseModel>;

    public class DeleteBusinessByIdHandler : IRequestHandler<DeleteBusinessByIdCommand, ResponseModel>
    {
        private readonly IBusinessRepository _repository;

        public DeleteBusinessByIdHandler(IBusinessRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseModel> Handle(DeleteBusinessByIdCommand request, CancellationToken cancellationToken)
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

using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;

namespace JobBoard.Application.Command
{
    public record DeleteBusinessByIdCommand(long Id) : DeleteEntityByIdCommand(Id);

    public class DeleteBusinessByIdHandler : DeleteEntityByIdHandler<Business, DeleteBusinessByIdCommand>
    {
        public DeleteBusinessByIdHandler(IBusinessRepository repository) : base(repository)
        {
        }
    }
}

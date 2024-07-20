using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;

namespace JobBoard.Application.Command
{
    public record DeleteJobOfferTagByIdCommand(long Id) : DeleteEntityByIdCommand(Id);

    public class DeleteJobOfferTagByIdHandler : DeleteEntityByIdHandler<JobOfferTag, DeleteJobOfferTagByIdCommand>
    {
        public DeleteJobOfferTagByIdHandler(IJobOfferTagRepository repository) : base(repository)
        {
        }
    }
}

using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;

namespace JobBoard.Application.Command
{
    public record DeleteJobOfferByIdCommand(long Id) : DeleteEntityByIdCommand(Id);

    public class DeleteJobOfferByIdHandler : DeleteEntityByIdHandler<JobOffer, DeleteJobOfferByIdCommand>
    {
        public DeleteJobOfferByIdHandler(IJobOfferRepository repository) : base(repository)
        {
        }
    }
}

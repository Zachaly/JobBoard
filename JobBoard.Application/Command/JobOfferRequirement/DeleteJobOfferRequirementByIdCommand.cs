using JobBoard.Application.Command.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;

namespace JobBoard.Application.Command
{
    public record DeleteJobOfferRequirementByIdCommand(long Id) : DeleteEntityByIdCommand(Id);

    public class DeleteJobOfferRequirementByIdHandler : DeleteEntityByIdHandler<JobOfferRequirement, DeleteJobOfferRequirementByIdCommand>
    {
        public DeleteJobOfferRequirementByIdHandler(IJobOfferRequirementRepository repository) : base(repository)
        {
        }
    }
}

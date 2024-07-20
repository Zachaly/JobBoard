using FluentValidation;
using JobBoard.Application.Command.Abstraction;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferTag;

namespace JobBoard.Application.Command
{
    public class AddJobOfferTagCommand : AddJobOfferTagRequest, IAddEntityCommand
    {
    }

    public class AddJobOfferTagHandler : AddEntityHandler<JobOfferTag, AddJobOfferTagRequest, AddJobOfferTagCommand>
    {
        public AddJobOfferTagHandler(IJobOfferTagRepository repository, IJobOfferTagFactory factory, IValidator<AddJobOfferTagRequest> validator)
            : base(repository, factory, validator)
        {
        }
    }
}

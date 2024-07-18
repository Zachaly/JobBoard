using FluentValidation;
using JobBoard.Application.Command.Abstraction;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.JobOffer;

namespace JobBoard.Application.Command
{
    public class AddJobOfferCommand : AddJobOfferRequest, IAddEntityCommand
    {
    }

    public class AddJobOfferHandler : AddEntityHandler<JobOffer, AddJobOfferRequest, AddJobOfferCommand>
    {
        public AddJobOfferHandler(IJobOfferRepository repository, IJobOfferFactory factory,
            IValidator<AddJobOfferRequest> validator) : base(repository, factory, validator)
        {
        }
    }
}

using FluentValidation;
using JobBoard.Application.Command.Abstraction;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.JobOffer;

namespace JobBoard.Application.Command
{
    public class UpdateJobOfferCommand : UpdateJobOfferRequest, IUpdateEntityCommand
    {
    }

    public class UpdateJobOfferHandler : UpdateEntityHandler<JobOffer, UpdateJobOfferRequest, UpdateJobOfferCommand>
    {
        public UpdateJobOfferHandler(IJobOfferRepository repository, IJobOfferFactory factory,
            IValidator<UpdateJobOfferRequest> validator) : base(repository, factory, validator)
        {
        }
    }
}

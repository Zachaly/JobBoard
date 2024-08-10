using FluentValidation;
using JobBoard.Application.Command.Abstraction;
using JobBoard.Application.Factory.Abstraction;
using JobBoard.Database.Repository.Abstraction;
using JobBoard.Domain.Entity;
using JobBoard.Model.JobOfferApplication;

namespace JobBoard.Application.Command
{
    public class UpdateJobOfferApplicationCommand : UpdateJobOfferApplicationRequest, IUpdateEntityCommand
    {
    }

    public class UpdateJobOfferApplicationHandler : UpdateEntityHandler<JobOfferApplication, UpdateJobOfferApplicationRequest,
        UpdateJobOfferApplicationCommand>
    {
        public UpdateJobOfferApplicationHandler(IJobOfferApplicationRepository repository, IJobOfferApplicationFactory factory,
            IValidator<UpdateJobOfferApplicationRequest> validator) : base(repository, factory, validator)
        {
        }
    }
}

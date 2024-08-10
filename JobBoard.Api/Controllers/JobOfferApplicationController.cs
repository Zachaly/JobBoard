using JobBoard.Api.Dto;
using JobBoard.Api.Extensions;
using JobBoard.Application.Command;
using JobBoard.Model.JobOfferApplication;
using JobBoard.Model.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Controllers
{
    [Route("/api/job-offer-application")]
    [Authorize]
    public class JobOfferApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobOfferApplicationController(IMediator mediator)
        {
            _mediator = mediator;   
        }

        /// <summary>
        /// Returns list of job offer applications filtered by query
        /// </summary>
        /// <response code="200">List of applications</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<JobOfferApplicationModel>>> Get([FromQuery] GetJobOfferApplicationCommand query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Returns application with specified id
        /// </summary>
        /// <response code="200">Job offer application</response>
        /// <response code="404">Application not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<JobOfferApplicationModel>> GetById(long id)
        {
            var res = await _mediator.Send(new GetJobOfferApplicationByIdCommand(id));

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Adds new application with data given in request
        /// </summary>
        /// <response code="201">Application created</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> Post([FromForm] AddJobOfferApplicationDto request) 
        {
            var res = await _mediator.Send(request.ToCommand());

            return res.ReturnCreatedOrBadRequest();
        }

        /// <summary>
        /// Deletes application with specified id and its resume file
        /// </summary>
        /// <response code="204">Application successfully deleted</response>
        /// <response code="400">Invalid id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteById(long id)
        {
            var res = await _mediator.Send(new DeleteJobOfferApplicationByIdCommand(id));

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Returns resume file of application with specified id
        /// </summary>
        /// <response code="200">Resume file</response>
        /// <response code="404">Application not found</response>
        [HttpGet("{id}/resume")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [AllowAnonymous]
        public async Task<ActionResult> GetResume(long id)
        {
            var res = await _mediator.Send(new GetJobOfferApplicationResumeByIdCommand(id));

            if(res is null)
            {
                return new NotFoundResult();
            }

            return new FileStreamResult(res, "application/pdf");
        }
    }
}

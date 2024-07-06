using JobBoard.Api.Constants;
using JobBoard.Api.Extensions;
using JobBoard.Application.Command;
using JobBoard.Model.JobOfferRequirement;
using JobBoard.Model.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Controllers
{
    [Route("/api/job-offer-requirement")]
    public class JobOfferRequirementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobOfferRequirementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of job offer requirements
        /// </summary>
        /// <response code="200">List of job offer requirements</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<JobOfferRequirementModel>>> Get([FromQuery] GetJobOfferRequirementCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        /// <summary>
        /// Adds new job offer requirement with data given in request
        /// </summary>
        /// <response code="201">Requirement created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize(Policy = AuthPolicyNames.Company)]
        public async Task<ActionResult<ResponseModel>> Post(AddJobOfferRequirementCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest();
        }

        /// <summary>
        /// Updates job offer requirement with data given in request
        /// </summary>
        /// <response code="204">Requirement updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Policy = AuthPolicyNames.Company)]
        public async Task<ActionResult<ResponseModel>> Update(UpdateJobOfferRequirementCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes job offer requirement with specified id
        /// </summary>
        /// <response code="204">Requirement deleted successfully</response>
        /// <response code="400">Invalid id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Policy = AuthPolicyNames.Company)]
        public async Task<ActionResult<ResponseModel>> DeleteById(long id)
        {
            var res = await _mediator.Send(new DeleteJobOfferRequirementByIdCommand(id));

            return res.ReturnNoContentOrBadRequest();
        }
    }
}

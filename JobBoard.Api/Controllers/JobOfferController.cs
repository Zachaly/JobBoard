using JobBoard.Api.Constants;
using JobBoard.Api.Extensions;
using JobBoard.Application.Command;
using JobBoard.Model.JobOffer;
using JobBoard.Model.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Controllers
{
    [Route("/api/job-offer")]
    public class JobOfferController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobOfferController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of job offers filtered by query
        /// </summary>
        /// <response code="200">List of offers</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<JobOfferModel>>> Get([FromQuery] GetJobOfferCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        /// <summary>
        /// Returns job offer with specified id
        /// </summary>
        /// <response code="200">Job offer model</response>
        /// <response code="404">No offer with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<JobOfferModel>> GetById(long id)
        {
            var res = await _mediator.Send(new GetJobOfferByIdCommand(id));

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Adds new job offer with data given in request
        /// </summary>
        /// <response code="201">Job offer created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize(Policy = AuthPolicyNames.Company)]
        public async Task<ActionResult<ResponseModel>> Post(AddJobOfferCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest();
        }

        /// <summary>
        /// Updates job offer with data given in request
        /// </summary>
        /// <response code="204">Job offer updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Policy = AuthPolicyNames.Company)]
        public async Task<ActionResult<ResponseModel>> Update(UpdateJobOfferCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes job offer with specified id
        /// </summary>
        /// <response code="204">Job offer removed successfully</response>
        /// <response code="400">Invalid id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Policy = AuthPolicyNames.Company)]
        public async Task<ActionResult<ResponseModel>> DeleteById(long id)
        {
            var res = await _mediator.Send(new DeleteJobOfferByIdCommand(id));

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Returns number of job offers filtered by request
        /// </summary>
        /// <response code="200">Number of job offers</response>
        [HttpGet("count")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<int>> GetCount([FromQuery] GetJobOfferCountCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(res);
        }
    }
}

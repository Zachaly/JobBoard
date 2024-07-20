using JobBoard.Api.Constants;
using JobBoard.Api.Extensions;
using JobBoard.Application.Command;
using JobBoard.Model.JobOfferTag;
using JobBoard.Model.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Controllers
{
    [Route("/api/job-offer-tag")]
    public class JobOfferTagController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobOfferTagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of job offer tags filtered by request
        /// </summary>
        /// <response code="200">List of tags</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<JobOfferTagModel>>> Get([FromQuery] GetJobOfferTagCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        /// <summary>
        /// Adds new job offer tag with data given in request
        /// </summary>
        /// <response code="201">Tag created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [Authorize(Policy = AuthPolicyNames.Company)]
        public async Task<ActionResult<ResponseModel>> Post(AddJobOfferTagCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest();
        }

        /// <summary>
        /// Returns number of job offer tags filtered by request
        /// </summary>
        /// <response code="200">Number of tags</response>
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount([FromQuery] GetJobOfferTagCountCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        /// <summary>
        /// Deletes job offer tag with specified id
        /// </summary>
        /// <response code="204">Tag deleted successfully</response>
        /// <response code="400">Invalid id</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = AuthPolicyNames.Company)]
        public async Task<ActionResult<ResponseModel>> DeleteById(long id)
        {
            var res = await _mediator.Send(new DeleteJobOfferTagByIdCommand(id));

            return res.ReturnNoContentOrBadRequest();
        }
    }
}

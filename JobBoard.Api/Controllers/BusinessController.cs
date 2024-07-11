using JobBoard.Api.Constants;
using JobBoard.Api.Extensions;
using JobBoard.Application.Command;
using JobBoard.Model.Business;
using JobBoard.Model.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Controllers
{
    [Route("/api/business")]
    public class BusinessController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BusinessController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of businesses
        /// </summary>
        /// <response code="200">List of businesses</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<BusinessModel>>> Get([FromQuery] GetBusinessCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        /// <summary>
        /// Adds new business with data given in request
        /// </summary>
        /// <response code="201">Business created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize(Policy = AuthPolicyNames.Admin)]
        public async Task<ActionResult<ResponseModel>> Post(AddBusinessCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest();
        }

        /// <summary>
        /// Updates business with data given in request
        /// </summary>
        /// <response code="204">Business updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Policy = AuthPolicyNames.Admin)]
        public async Task<ActionResult<ResponseModel>> Update(UpdateBusinessCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes business with specified id
        /// </summary>
        /// <response code="204">Business deleted successfully</response>
        /// <response code="400">Invalid id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Policy = AuthPolicyNames.Admin)]
        public async Task<ActionResult<ResponseModel>> DeleteById(long id)
        {
            var res = await _mediator.Send(new DeleteBusinessByIdCommand(id));

            return res.ReturnNoContentOrBadRequest();
        }
    }
}

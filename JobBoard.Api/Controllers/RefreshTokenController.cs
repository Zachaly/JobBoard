using JobBoard.Api.Extensions;
using JobBoard.Application.Command;
using JobBoard.Model.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Controllers
{
    [Route("/api/refresh-token")]
    public class RefreshTokenController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RefreshTokenController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Refreshes admin access token
        /// </summary>
        /// <response code="200">New access and refresh token</response>
        /// <response code="400">Invalid token sent</response>
        [HttpPost("admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<LoginResponse>> RefreshAdminToken(RefreshAdminTokenCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnOkOrBadRequest();
        }

        /// <summary>
        /// Refreshes company access token
        /// </summary>
        /// <response code="200">New access and refresh token</response>
        /// <response code="400">Invalid token sent</response>
        [HttpPost("company")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<LoginResponse>> RefreshCompanyToken(RefreshCompanyTokenCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnOkOrBadRequest();
        }

        /// <summary>
        /// Refreshes employee access token
        /// </summary>
        /// <response code="200">New access and refresh token</response>
        /// <response code="400">Invalid token sent</response>
        [HttpPost("employee")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<LoginResponse>> RefreshEmployeeToken(RefreshEmployeeTokenCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnOkOrBadRequest();
        }

        /// <summary>
        /// Revokes specified refresh token
        /// </summary>
        /// <response code="204">Token revoken or non existent</response>
        [HttpPatch("admin/revoke")]
        [ProducesResponseType(204)]
        public async Task<ActionResult<ResponseModel>> RevokeAdminToken(RevokeAdminTokenCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Revokes specified refresh token
        /// </summary>
        /// <response code="204">Token revoken or non existent</response>
        [HttpPatch("company/revoke")]
        [ProducesResponseType(204)]
        public async Task<ActionResult<ResponseModel>> RevokeCompanyToken(RevokeCompanyTokenCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Revokes specified refresh token
        /// </summary>
        /// <response code="204">Token revoken or non existent</response>
        [HttpPatch("employee/revoke")]
        [ProducesResponseType(204)]
        public async Task<ActionResult<ResponseModel>> RevokeEmployeeToken(RevokeEmployeeTokenCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}

using JobBoard.Api.Constants;
using JobBoard.Api.Extensions;
using JobBoard.Application.Command;
using JobBoard.Model.AdminAccount;
using JobBoard.Model.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Controllers
{
    [Route("/api/admin-account")]
    [Authorize(Policy = AuthPolicyNames.Admin)]
    public class AdminAccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of admin account
        /// </summary>
        /// <response code="200">List of admin accounts</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<AdminAccountModel>>> Get([FromQuery] GetAdminAccountCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        /// <summary>
        /// Returns admin account with specified id
        /// </summary>
        /// <response code="200">Admin accout model</response>
        /// <response code="404">No account with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AdminAccountModel>> GetById(long id)
        {
            var res = await _mediator.Send(new GetAdminAccountByIdCommand(id));

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Creates new admin account with data given in request
        /// </summary>
        /// <response code="201">Account created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> Post(AddAdminAccountCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest();
        }

        /// <summary>
        /// Returns access token and id of specified user
        /// </summary>
        /// <response code="200">Login successfull</response>
        /// <response code="400">Invalid login data</response>
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login(AdminLoginCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnOkOrBadRequest();
        }
    }
}

using JobBoard.Api.Extensions;
using JobBoard.Application.Command;
using JobBoard.Model.CompanyAccount;
using JobBoard.Model.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Controllers
{
    [Route("/api/company-account")]
    public class CompanyAccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of companies filtered by request
        /// </summary>
        /// <response code="200">List of companies</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<CompanyModel>>> Get([FromQuery] GetCompanyAccountCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        /// <summary>
        /// Returns company with specified id
        /// </summary>
        /// <response code="200">Company model</response>
        /// <response code="404">Company not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CompanyModel>> GetById(long id)
        {
            var res = await _mediator.Send(new GetCompanyAccountByIdCommand(id));

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Adds new company account with data from request
        /// </summary>
        /// <response code="201">Account created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> Add(AddCompanyAccountCommand command)
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
        public async Task<ActionResult<LoginResponse>> Login(CompanyLoginCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnOkOrBadRequest();
        }
    }
}

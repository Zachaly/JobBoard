﻿using JobBoard.Api.Extensions;
using JobBoard.Application.Command;
using JobBoard.Model.EmployeeAccount;
using JobBoard.Model.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Controllers
{
    [Route("/api/employee-account")]
    public class EmployeeAccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of employee account filtered by request
        /// </summary>
        /// <response code="200">List of accounts</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<EmployeeAccountModel>>> Get([FromQuery] GetEmployeeAccountCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        /// <summary>
        /// Returns employee account with specified id
        /// </summary>
        /// <response code="200">Employee account model</response>
        /// <response code="404">Not account with specified id found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<EmployeeAccountModel>> GetById(long id)
        {
            var res = await _mediator.Send(new GetEmployeeAccountByIdCommand(id));

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Adds new employee account with data given in request
        /// </summary>
        /// <response code="201">Account created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> Post(AddEmployeeAccountCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest();
        }
    }
}

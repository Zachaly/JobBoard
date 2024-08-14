using JobBoard.Api.Constants;
using JobBoard.Api.Extensions;
using JobBoard.Application.Command;
using JobBoard.Model.EmployeeResume;
using JobBoard.Model.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Controllers
{
    [Route("/api/employee-resume")]
    [Authorize(Policy = AuthPolicyNames.Employee)]
    public class EmployeeResumeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeResumeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of resumes filtered by query
        /// </summary>
        /// <response code="200">List of resumes</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<EmployeeResumeModel>>> Get([FromQuery] GetEmployeeResumeCommand query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Returns resume with specified id
        /// </summary>
        /// <response code="200">Resume data</response>
        /// <response code="404">Resume not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<EmployeeResumeModel>> GetById(long id)
        {
            var res = await _mediator.Send(new GetEmployeeResumeByIdCommand(id));

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Returns pdf filestream of resume with specified id
        /// </summary>
        /// <response code="200">Pdf</response>
        /// <response code="404">Resumse not found</response>
        [HttpGet("{id}/file")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [AllowAnonymous]
        public async Task<ActionResult> GetFileById(long id)
        {
            var res = await _mediator.Send(new GetEmployeeResumeFileByIdCommand(id));
            
            if(res.File is null)
            {
                return new NotFoundResult();
            }

            return File(res.File, "application/pdf", res.Name);
        }

        /// <summary>
        /// Adds new resume with data specified in request
        /// </summary>
        /// <response code="201">Resume creates successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> Post(AddEmployeeResumeCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest();
        }
        
        /// <summary>
        /// Deletes resume with specified id
        /// </summary>
        /// <response code="204">Resume deleted successfully</response>
        /// <response code="400">Invalid id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteById(long id)
        {
            var res = await _mediator.Send(new DeleteEmployeeResumeByIdCommand(id));

            return res.ReturnNoContentOrBadRequest();
        }
    }
}

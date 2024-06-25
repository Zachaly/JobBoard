using JobBoard.Model.Response;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Api.Extensions
{
    public static class ResponseModelExtensions
    {
        public static ActionResult ReturnCreatedOrBadRequest(this ResponseModel response)
        {
            if(response.IsSuccess)
            {
                return new CreatedResult("", response);
            }

            return new BadRequestObjectResult(response);
        }

        public static ActionResult<TResult> ReturnOkOrNotFound<TResult>(TResult? res) where TResult : class
        {
            if (res == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(res);
        }

        public static ActionResult ReturnNoContentOrBadRequest(this ResponseModel response)
        {
            if(response.IsSuccess)
            {
                return new NoContentResult();
            }

            return new BadRequestObjectResult(response);
        }

        public static ActionResult ReturnOkOrBadRequest(this ResponseModel response)
        {
            if (response.IsSuccess)
            {
                return new OkObjectResult(response);
            }

            return new BadRequestObjectResult(response);
        }
    }
}

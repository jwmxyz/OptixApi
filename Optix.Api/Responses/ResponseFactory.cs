using Optix.Api.Factory;
using Optix.ErrorManagement.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Optix.Api.DTO;

namespace Optix.Api.Responses;

public class ResponseFactory : IResponseFactory
{
    public IActionResult CreateResponse<T>(PagingResult<T> results)
    {
        return new OkObjectResult(new OptixStandardResult(results));
    }

    /// <see cref="IResponseFactory.CreateResponse{T}({T})"/>
    public IActionResult CreateResponse<T>(T exception) where T : Exception
    {
        return exception switch
        {
            InvalidSearchParametersException => new BadRequestObjectResult(
                new OptixStandardResult(null, true, exception.Message))
            {
                StatusCode = StatusCodes.Status400BadRequest
            },
            MovieNotFoundException => new NotFoundObjectResult(
                new OptixStandardResult(null, true, exception.Message)
            ),
            _ => new ObjectResult(
                new OptixStandardResult(null, true, exception.Message))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }
}